using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step1Model : Step1PostModel, IStepModel
    {
        public string BeginDateString { get { return this.BeginDate.ToString("yyyy-MM-dd"); } }

        public string EndDateString { get { return this.EndDate.ToString("yyyy-MM-dd"); } }

        public int StepNo
        {
            get { return 1; }
        }

        public TravelChildType TravelChildType { get; set; }
    }
}