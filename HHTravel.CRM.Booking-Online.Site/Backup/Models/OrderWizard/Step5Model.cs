using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;

namespace HHTravel.CRM.Booking_Online.Site.Models.OrderWizard
{
    public class Step5Model : Step5PostModel, IStepModel
    {
        public Step5Model()
        {
            this.FlightsSegmentPlans = new List<FlightsPlanModel>();
            this.FlightsSegments = new List<FlightsSegmentModel>();
            this.PassengerModelList = new List<PassengerModel>();
        }

        public int AdultCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal AveragePrice { get { return Math.Round(this.TotalPrice / (this.AdultCount + this.ChildCount)); } }

        public BasicInfoModel BasicInfoModel { get; set; }

        public DateTime BeginDate { get; set; }

        public int BeginSubstepNo { get; set; }

        public int ChildCount { get; set; }

        public int Days { get { return (this.EndDate - this.BeginDate).Days; } }

        public HotelSegmentModel DelayHotelSegment { get; set; }

        public DateTime EndDate { get; set; }

        public List<FlightsPlanModel> FlightsSegmentPlans { get; set; }

        public List<FlightsSegmentModel> FlightsSegments { get; set; }

        public GroundServiceModel GroundService { get; set; }

        public HotelSegmentModel HotelSegment { get; set; }

        public List<PassengerModel> PassengerModelList { get; set; }

        public string ProductFeature { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        public string SpecialRequirement { get; set; }

        public int StepNo
        {
            get { return 5; }
        }
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 是否用于发送邮件
        /// </summary>
        public bool UsedByEmail { get; set; }

        [ContractInvariantMethod]
        public void Invariant()
        {
            Contract.Requires(this.HotelSegment != null);
            Contract.Requires(this.GroundService != null);
            Contract.Requires(this.FlightsSegments == null || (this.FlightsSegments.Any() && this.FlightsSegments.Any()));
        }
    }
}