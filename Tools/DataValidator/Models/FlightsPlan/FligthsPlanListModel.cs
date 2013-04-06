using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.FlightsPlanService;
using Newtonsoft.Json;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace DataValidator.Models.FlightsPlan
{
    public class FligthsPlanListModel
    {
        public string DbServer { get; set; }

        public bool IsShowPlansJson { get; set; }

        public FlightsPlanSearchCondition SearchCondition { get; set; }

        public List<FlightsPlanModel> PlanModels { get; set; }

        public string SearchConditionJson
        {
            get
            {
                this.SearchCondition.FlightsSegments.ForEach(seg => seg.FlightsPlans = null);
                var str = JsonConvert.SerializeObject(this.SearchCondition);
                return str;
            }
        }
    }
}