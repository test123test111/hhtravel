using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.Infrastructure;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step4ModelBuilder : IModelBuilder<Step4Model, object>
    {
        private WizardContext _context;
        private decimal _totalPrice;

        public Step4ModelBuilder(WizardContext context, decimal totalPrice)
        {
            _context = context;
            _totalPrice = totalPrice;
        }

        public Step4Model CreateFrom(object entity)
        {
            var adultCount = _context.Step1Model.AdultCount;
            var childCount = _context.Step1Model.ChildCount ?? 0;
            var personCount = adultCount + childCount;

            var model = new Step4Model
            {
                PassengerModelList = (from i in new IntRange(1, personCount)
                                      select new PassengerModel { }).ToList()
            };
            model = this.Rebuild(model);

            return model;
        }

        public Step4Model Rebuild(Step4Model model)
        {
            var adultCount = _context.Step1Model.AdultCount;
            var childCount = _context.Step1Model.ChildCount ?? 0;

            model.BeginSubstepNo = _context.GetBeginSubstepNo(model.StepNo);
            model.ProductNo = _context.ProductNo;
            model.ProductName = _context.ProductName;
            model.AdultCount = adultCount;
            model.ChildCount = childCount;
            model.TotalPrice = _totalPrice;

            return model;
        }
    }
}