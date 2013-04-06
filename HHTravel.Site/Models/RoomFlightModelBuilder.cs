using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HHTravel.DomainModel;
using HHTravel.Infrastructure.Web.Mvc;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models
{
    public class RoomFlightModelBuilder : IModelBuilder<RoomFlightModel, object>
    {
        private int _month;
        private Product _product;
        private DateTime _beginDate;
        private int _year;
        private Func<int, DateTime, IList<Ticket>> _getTickets;
        private Func<int, DateTime, IList<RoomClass>> _getRoomClasses;
        private Func<int, TicketSegment> _getTicketSegment;
        private Func<int, IList<HotelSegment>> _getHotelSegments;

        public RoomFlightModelBuilder(Product product, DateTime beginDate, int year, int month,
            Func<int, TicketSegment> getTicketSegment,
            Func<int, IList<HotelSegment>> getHotelSegments,
            Func<int, DateTime, IList<Ticket>> getTickets, 
            Func<int, DateTime, IList<RoomClass>> getRoomClasses)
        {
            _product = product;
            _beginDate = beginDate;
            _year = year;
            _month = month;

            _getTicketSegment = getTicketSegment;
            _getHotelSegments = getHotelSegments;
            _getTickets = getTickets;
            _getRoomClasses = getRoomClasses;
        }

        public RoomFlightModel CreateFrom(object entity)
        {
            var pd = _product.GetPriceDate(_beginDate);
            var model = new RoomFlightModel
            {
                ProductNo = _product.ProductNo,
                ProductName = _product.ProductName,
                ProductPrice = pd.Price,
                Year = _year,
                Month = _month,
                DepartureDate = _beginDate,
                ReturnDate = _beginDate.AddDays(_product.Days - 1), // 计算预定的回程日期
                Days = _product.Days,                     // 暂未使用 2013.01.09
                Nights = _product.MinLodgingDays,         // TravelType3
                MaxPersonNum = _product.MaxPersonNum,     // TravelType1/4
            };

            model.PreOrderModel = new PreOrderModel
            {
                ProductId = _product.ProductId,
                ProductNo = _product.ProductNo,
                ProductName = _product.ProductName,
                DepartureDate = _beginDate,
                PlanReturnDate = _beginDate.AddDays(_product.Days - 1),
                Days = _product.Days,
                TravelType = _product.TravelType,
                ProductPrice = pd.Price,
            };

            model = this.Rebuild(model);

            return model;
        }

        public RoomFlightModel Rebuild(RoomFlightModel model)
        {
            TravelType travelType = model.PreOrderModel.TravelType;

            List<TicketModel> ticketModels;

            if (travelType == TravelType.TravelType2)
            {
                model.MinPersonNum = 2;

                var builder = new TicketSegmentModelBuilder(_beginDate);
                var ticketSegment = _getTicketSegment(_product.ProductId);
                model.TicketSegmentModel = builder.CreateFrom(ticketSegment);

                // 行程段1-*酒店1-*房型
                var builder2 = new HotelSegmentModelsBuilder(_beginDate);
                var hotelSegments = _getHotelSegments(_product.ProductId);
                model.HotelSegmentModelList = builder2.CreateFrom(hotelSegments);
                ticketModels = model.TicketSegmentModel.TicketModelList;

                model.PreOrderModel.TicketModels = ticketModels;

                // 获取默认选中的机票的Id
                model.PreOrderModel.SelectedTicketId = model.TicketSegmentModel.MinPriceTicketModule.TicketId;

                model.PreOrderModel.RoomClassPostModelList = (from seg in model.HotelSegmentModelList
                                                              from hotel in seg.HotelModelList
                                                              from rc in hotel.RoomClassModelList
                                                              select new RoomClassPostModel
                                                              {
                                                                  SegmentId = seg.SegmentId,
                                                                  HotelId = hotel.HotelId,
                                                                  RoomClassId = rc.RoomClassId,
                                                                  RoomClassName = rc.RoomClassName,
                                                                  BreakfastDinnerTip = rc.BreakfastDinnerTip,
                                                                  SegmentPrice = rc.SegmentPrice,
                                                                  SegmentPriceDelta = rc.SegmentPriceDelta,
                                                              }).ToList();
            }
            else
            {
                var builder = new TicketModelListBuilder(_beginDate, travelType);
                var tickets = _getTickets(_product.ProductId, _beginDate);
                ticketModels = builder.CreateFrom(tickets);

                List<RoomClassModel> roomClassModels;
                var queryRoomClass = from rc in _getRoomClasses(_product.ProductId, _beginDate)
                                     let priceDate = rc.GetPriceDate(_beginDate)
                                     where priceDate.Price > 0
                                     select new RoomClassModel
                                     {
                                         RoomClassId = rc.RoomClassId,
                                         RoomClassName = rc.RoomClassName,
                                         BreakfastDinnerTip = rc.BreakfastDinnerTip,
                                         SegmentPrice = priceDate.Price,
                                         StayPrice = priceDate.StayPricePerDay
                                     };
                roomClassModels = queryRoomClass.OrderBy(rc => rc.SegmentPrice).ToList();

                if (travelType == TravelType.TravelType3)
                {
                    model.PreOrderModel.OldDescription = _product.OldDescription;
                    model.PreOrderModel.RoomClassModels = roomClassModels;
                    model.PreOrderModel.StayRoomClassModels = roomClassModels;
                    model.PreOrderModel.TicketModels = ticketModels;
                }
                else if (travelType == TravelType.TravelType1 || travelType == TravelType.TravelType4)
                {
                    model.RoomClassModelList = roomClassModels;
                    model.TicketModelList = ticketModels;
                }
            }

            return model;
        }
    }
}