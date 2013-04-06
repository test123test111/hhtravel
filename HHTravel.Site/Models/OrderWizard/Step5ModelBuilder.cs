using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site.Models.OrderWizard
{
    public class Step5ModelBuilder : IModelBuilder<Step5Model, object>
    {
        private WizardContext _context;
        private string _productFeature;
        private HotelSegmentsModel _hotelSegmentsModel;
        private List<FlightsSegmentModel> _flightsSegmentModels;
        private FlightsPlanModelBuilder _filghtsPlanModelBuilder = new FlightsPlanModelBuilder();
        private List<GroundServiceModel> _groundServiceModels;

        public Step5ModelBuilder(WizardContext context, 
            string productFeature,
            HotelSegmentsModel hotelSegmentsModel,
            List<FlightsSegmentModel> flightsSegmentModels,
            List<GroundServiceModel> groundServiceModels)
        {
            _context = context;
            _productFeature = productFeature;
            _hotelSegmentsModel = hotelSegmentsModel;
            _flightsSegmentModels = flightsSegmentModels;
            _groundServiceModels = groundServiceModels;
        }

        public Step5Model CreateFrom(object entity)
        {
            var model = new Step5Model();
            model = this.Rebuild(model);
            
            return model;
        }

        public Step5Model Rebuild(Step5Model model)
        {
            decimal totalPrice = _context.Step2Model.IsChooseFlights ? _context.Step3Model.TotalPrice : _context.Step2Model.TotalPrice;

            model.ProductNo = _context.ProductNo;
            model.ProductName = _context.ProductName;
            model.AdultCount = _context.Step1Model.AdultCount;
            model.ChildCount = _context.Step1Model.ChildCount ?? 0;
            model.TotalPrice = totalPrice;
            model.BeginDate = _context.Step1Model.BeginDate;
            model.EndDate = _context.Step1Model.EndDate.AddDays(_context.Step1Model.DelayDays ?? 0);
            model.ProductFeature = _productFeature;
            model.PassengerModelList = (from p in _context.Step4Model.PassengerModelList
                                        where !string.IsNullOrEmpty(p.Name) || !string.IsNullOrEmpty(p.NameEn)
                                        select p).ToList();
            model.BasicInfoModel = _context.Step4Model.BasicInfoModel;
            model.SpecialRequirement = _context.Step4Model.SpecialRequirement;
            var hotelSegmentModelBuilder = new HotelSegmentModelBuilder(_context.Step2Model.HotelPostModel);

            model.HotelSegment = hotelSegmentModelBuilder.Rebuild(_hotelSegmentsModel.HotelSegmentModel);

            if (_context.Step2Model.DelayHotelPostModel != null)
            {
                hotelSegmentModelBuilder = new HotelSegmentModelBuilder(_context.Step2Model.DelayHotelPostModel);
                model.DelayHotelSegment = hotelSegmentModelBuilder.Rebuild(_hotelSegmentsModel.DelayHotelSegmentModel);
            }

            if (_groundServiceModels.Any())
            {
                model.GroundService = (from gs in _groundServiceModels
                                       where gs.GroundServiceId == _context.Step2Model.SelectedGroundServiceId
                                       select gs).Single();
            }

            if (_context.Step2Model.IsChooseFlights)
            {
                model.FlightsSegments = _flightsSegmentModels;
                var plans = (from json in _context.Step3Model.FlightsSegmentPlanJsons
                             let plan = JsonConvert.DeserializeObject<FlightsPlan>(json)
                             select plan).ToList();
                model.FlightsSegmentPlans = _filghtsPlanModelBuilder.CreateFrom(plans);
            }

            return model;
        }
    }
}