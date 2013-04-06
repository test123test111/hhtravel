using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Site.Models.OrderWizard
{
    public class Step3Model : Step3PostModel, IStepModel
    {
        public Step3Model()
        {
            this.FlightNos = new List<string>();
            this.FlightsSegmentPlanJsons = new List<string>();
        }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal AveragePrice { get { return Math.Round(this.TotalPrice / (this.AdultCount + this.ChildCount)); } }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public List<FlightsSegmentModel> FlightsSegments { get; set; }

        public int StepNo
        {
            get { return 3; }
        }

        public int BeginSubstepNo { get; set; }
    }
}