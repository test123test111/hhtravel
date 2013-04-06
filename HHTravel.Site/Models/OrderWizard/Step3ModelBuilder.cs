using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics.Contracts;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step3ModelBuilder : IModelBuilder<Step3Model, object>
    {
        private List<FlightsSegmentModel> _flightsSegmentModels;
        private WizardContext _context;

        public Step3ModelBuilder(WizardContext context, List<FlightsSegmentModel> flightsSegmentModels)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.Step1Model != null);
            Contract.Requires(context.Step2Model != null);
            Contract.Requires(context.Step2Model.TotalPrice > 0);
            Contract.Requires(_flightsSegmentModels != null && _flightsSegmentModels.Count > 0);

            _context = context;
            _flightsSegmentModels = flightsSegmentModels;
        }

        public Step3Model CreateFrom(object entity)
        {
            var model = new Step3Model();
            model = this.Rebuild(model);

            return model;
        }

        public Step3Model Rebuild(Step3Model model)
        {
            model.BeginSubstepNo = _context.GetBeginSubstepNo(model.StepNo);
            model.ProductNo = _context.ProductNo;
            model.ProductName = _context.ProductName;
            model.AdultCount = _context.Step1Model.AdultCount;
            model.ChildCount = _context.Step1Model.ChildCount ?? 0;
            model.TotalPrice = _context.Step2Model.TotalPrice;
            model.FlightsSegments = _flightsSegmentModels;

            return model;
        }
    }
}