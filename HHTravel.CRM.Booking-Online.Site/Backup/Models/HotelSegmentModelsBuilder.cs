using System;
using System.Collections.Generic;
using System.Linq;
using HHTravel.CRM.Booking_Online.Infrastructure.Web.Mvc;
using HHTravel.CRM.Booking_Online.DomainModel;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class HotelSegmentModelsBuilder : IModelBuilder<List<HotelSegmentModel>, IList<HotelSegment> >
    {
        private DateTime _beginDate;

        public HotelSegmentModelsBuilder(DateTime beginDate)
        {
            _beginDate = beginDate;
        }

        public List<HotelSegmentModel> CreateFrom(IList<HotelSegment> entity)
        {
            var hotelSegmentModels = new List<HotelSegmentModel>();

            DateTime checkInDate = _beginDate;
            DateTime checkoutDate;
            foreach (var seg in entity)
            {
                checkoutDate = checkInDate.AddDays(seg.Nights);
                decimal minSegmentPrice = seg.GetPriceDate(checkInDate).Price;
                var model = new HotelSegmentModel
                                {
                                    SegmentId = seg.SegmentId,
                                    CheckinDate = checkInDate,
                                    CheckoutDate = checkInDate.AddDays(seg.Nights),
                                    DestinationCity = seg.City,
                                    HotelModelList = (from hotel in seg.HotelList
                                                      select new HotelModel
                                                      {
                                                          HotelId = hotel.HotelId,
                                                          HotelName = hotel.HotelName,
                                                          RoomClassModelList = (from room in hotel.RoomClassList
                                                                                let segmentPriceInRoom = room.GetTotalPrice(checkInDate, checkoutDate)
                                                                                let segmentPriceDelta = segmentPriceInRoom - minSegmentPrice
                                                                                select new RoomClassModel
                                                                                {
                                                                                    RoomClassId = room.RoomClassId,
                                                                                    RoomClassName = room.RoomClassName,
                                                                                    BreakfastDinnerTip = room.BreakfastDinnerTip,
                                                                                    SegmentPrice = segmentPriceInRoom,
                                                                                    SegmentPriceDelta = segmentPriceDelta,
                                                                                }).ToList()
                                                      }).ToList(),
                                    MinTotalPrice = minSegmentPrice,
                                    Nights = seg.Nights,
                                };
                hotelSegmentModels.Add(model);

                // 为下一行程段准备入住日期
                checkInDate = model.CheckoutDate;

                /* 为了避免用户换选其他房型时需要先将默认选择的房型的房间数改为0，取消此逻辑

                // 将各行程段最低价的房型的房间数设为1
                var rcListWithMinPrice = (from h in model.HotelModelList
                                          from rc in h.RoomClassModelList
                                          where (rc.Price * model.Nights) == model.MinTotalPrice
                                          select rc).ToList();
                rcListWithMinPrice.ForEach(rc => rc.RoomCountSelectList = SelectListFactory.CreateRoomCountSelectList(1));
                 */
            }

            return hotelSegmentModels;
        }

        public List<HotelSegmentModel> Rebuild(List<HotelSegmentModel> model)
        {
            throw new NotImplementedException();
        }
    }
}