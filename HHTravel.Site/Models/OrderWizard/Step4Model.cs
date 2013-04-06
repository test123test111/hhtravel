using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step4Model : Step4PostModel, IStepModel
    {
        public Step4Model()
        {
            this.PassengerModelList = new List<PassengerModel>();
        }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal AveragePrice { get { return Math.Round(this.TotalPrice / (this.AdultCount + this.ChildCount)); } }

        public int StepNo
        {
            get { return 4; }
        }

        public int BeginSubstepNo { get; set; }
    }
}