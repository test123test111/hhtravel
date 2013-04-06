using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.Infrastructure.Crosscutting;
using System.Diagnostics.Contracts;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step2ModelBuilder : IModelBuilder<Step2Model, object>
    {
        private WizardContext _context;
        private HotelSegmentsModel _hotelSegmentsModel;
        private List<FlightsSegmentModel> _flightsSegmentModels;
        private List<GroundServiceModel> _groundServiceModels;

        public Step2ModelBuilder(WizardContext context,
            HotelSegmentsModel hotelSegmentsModel,
            List<FlightsSegmentModel> flightsSegmentModels,
            List<GroundServiceModel> groundServiceModels)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.Step1Model != null);
            Contract.Requires(hotelSegmentsModel != null);
            Contract.Requires(flightsSegmentModels != null && flightsSegmentModels.Count > 0);
            Contract.Requires(groundServiceModels != null);

            _context = context;
            _groundServiceModels = groundServiceModels;
            _hotelSegmentsModel = hotelSegmentsModel;
            _flightsSegmentModels = flightsSegmentModels;
        }

        public Step2Model CreateFrom(object entity)
        {
            var model = new Step2Model
            {
                FlightSeat = FlightSeat.公务舱,
            };
            model = this.Rebuild(model);

            return model;
        }

        public Step2Model Rebuild(Step2Model model)
        {
            model.BeginSubstepNo = _context.GetBeginSubstepNo(model.StepNo);
            model.ProductNo = _context.ProductNo;
            model.ProductName = _context.ProductName;
            model.HotelSegment = _hotelSegmentsModel.HotelSegmentModel;
            model.DelayHotelSegment = _hotelSegmentsModel.DelayHotelSegmentModel;
            model.GroundServiceModels = _groundServiceModels;
            model.FlightsSegmentModels = _flightsSegmentModels;
            model.AdultCount = _context.Step1Model.AdultCount;
            model.ChildCount = _context.Step1Model.ChildCount ?? 0;

            return model;
        }
    }
}