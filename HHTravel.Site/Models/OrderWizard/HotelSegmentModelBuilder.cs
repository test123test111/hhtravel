using System;
using System.Collections.Generic;
using System.Linq;
using HHTravel.DomainModel;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models.OrderWizard
{
    public class HotelSegmentModelBuilder : IModelBuilder<HotelSegmentModel, HotelSegment>
    {
        private DateTime _beginDate;

        private int? _delayNights;

        private HotelPostModel _hotelPostModel;

        public HotelSegmentModelBuilder(DateTime beginDate, int? delayNights = null)
        {
            _beginDate = beginDate;
            _delayNights = delayNights;
        }
        public HotelSegmentModelBuilder(HotelPostModel hotelPostModel)
        {
            _hotelPostModel = hotelPostModel;
        }

        public HotelSegmentModel CreateFrom(HotelSegment entity)
        {
            var hotelSegment = entity;
            var beginDate = _beginDate;

            if (hotelSegment == null)
            {
                return null;
            }

            int nights = _delayNights ?? hotelSegment.Nights;
            DateTime endDate = beginDate.AddDays(nights);

            // 延长住宿的段最低价需要实时计算
            decimal minSegmentPrice = hotelSegment.HotelSegmentType == HotelSegmentType.延长住宿 ?
                hotelSegment.HotelList.Min(h => h.RoomClassList.Min(r => r.GetTotalPrice(beginDate, endDate))) :
                hotelSegment.GetPriceDate(beginDate).Price;

            var hotelSegModel = new HotelSegmentModel
            {
                Nights = nights,
                CheckinDate = beginDate,
                CheckoutDate = endDate,
                HotelSegmentType = hotelSegment.HotelSegmentType,
                Description = hotelSegment.Description,
                HotelModelList = (from hotel in hotelSegment.HotelList
                                  where hotel.RoomClassList.Count > 0
                                  let minSegmentPriceInHotel = hotel.RoomClassList.Min(r => hotelSegment.HotelSegmentType != HotelSegmentType.线路 ?
                                                                                            r.GetTotalPrice(beginDate, endDate) : r.GetPriceDate(beginDate).Price)
                                  select new HotelModel
                                  {
                                      HotelId = hotel.HotelId,
                                      HotelName = hotel.HotelName,
                                      Description = hotel.Description,
                                      RoomClassModelList = (from room in hotel.RoomClassList
                                                            // 酒店段的段最低价计算需要考虑变价
                                                            let segmentPriceInRoom = hotelSegment.HotelSegmentType != HotelSegmentType.线路 ?
                                                                                     room.GetTotalPrice(beginDate, endDate) : room.GetPriceDate(beginDate).Price
                                                            let segmentPriceDelta = segmentPriceInRoom - minSegmentPrice
                                                            select new RoomClassModel
                                                            {
                                                                RoomClassId = room.RoomClassId,
                                                                RoomClassName = room.RoomClassName,
                                                                BreakfastDinnerTip = "含早",
                                                                MaxOccupancy = room.MaxOccupancy,
                                                                SegmentPrice = segmentPriceInRoom,

                                                                // 计算差价
                                                                SegmentPriceDelta = segmentPriceDelta,
                                                                WithLowestPrice = (segmentPriceInRoom != 0 && segmentPriceDelta == 0),
                                                            }).ToList(),
                                      Checked = (minSegmentPriceInHotel == minSegmentPrice),
                                  }).ToList(),
            };

            return hotelSegModel;
        }

        public HotelSegmentModel Rebuild(HotelSegmentModel model)
        {
            model.HotelModelList = (from h in model.HotelModelList
                                    where h.HotelId == _hotelPostModel.HotelId
                                    select new HotelModel
                                    {
                                        HotelId = h.HotelId,
                                        HotelName = h.HotelName,
                                        Description = h.Description,
                                        RoomClassModelList = (from r in h.RoomClassModelList
                                                              join sr in _hotelPostModel.RoomClassPostModels
                                                              on r.RoomClassId equals sr.RoomClassId
                                                              where sr.RoomCount > 0
                                                              let rc = new RoomClassModel
                                                              {
                                                                  RoomClassId = r.RoomClassId,
                                                                  RoomClassName = r.RoomClassName,
                                                                  BreakfastDinnerTip = r.BreakfastDinnerTip,
                                                                  MaxOccupancy = r.MaxOccupancy,
                                                                  SegmentPriceDelta = r.SegmentPriceDelta,
                                                                  Count = sr.RoomCount,
                                                              }
                                                              select rc).ToList()
                                    }).ToList();
     
            return model;
        }
    }
}