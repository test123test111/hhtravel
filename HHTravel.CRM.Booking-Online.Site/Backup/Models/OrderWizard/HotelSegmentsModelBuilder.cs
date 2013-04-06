using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site.Models.OrderWizard
{
    public class HotelSegmentsModelBuilder : IModelBuilder<HotelSegmentsModel, IList<HotelSegment>>
    {
        private DateTime _beginDate;
        private int? _delayNights;
        public HotelSegmentsModelBuilder(DateTime beginDate, int? delayNights)
        {
            _beginDate = beginDate;
            _delayNights = delayNights;
        }

        public HotelSegmentsModel CreateFrom(IList<HotelSegment> entity)
        {
            HotelSegmentsModel model = new HotelSegmentsModel();
            var beginDate = _beginDate;
            var delayNights = _delayNights;
            var _hotelSegments = entity;

            var hotelSegment = _hotelSegments.FirstOrDefault(hs => hs.HotelSegmentType != HotelSegmentType.延长住宿);
            var builder = new HotelSegmentModelBuilder(beginDate);
            model.HotelSegmentModel = builder.CreateFrom(hotelSegment);

            // 没有行程段，何来延住段
            if (model.HotelSegmentModel == null)
            {
                model.DelayHotelSegmentModel = null;
            }

            if (!delayNights.HasValue || delayNights.Value <= 0)
            {
                model.DelayHotelSegmentModel = null;
            }
            else
            {
                hotelSegment = _hotelSegments.FirstOrDefault(hs => hs.HotelSegmentType == HotelSegmentType.延长住宿);
                builder = new HotelSegmentModelBuilder(model.HotelSegmentModel.CheckoutDate, delayNights);
                model.DelayHotelSegmentModel = builder.CreateFrom(hotelSegment);
            }

            return model;
        }

        public HotelSegmentsModel Rebuild(HotelSegmentsModel model)
        {
            throw new NotImplementedException();
        }
    }
}