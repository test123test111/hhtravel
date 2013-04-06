using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.Infrastructure.Crosscutting;
using System.Web.Mvc;

namespace DataValidator.Models.FlightsPlan
{
    public class FlightsPlanSearchModel
    {
        public static IEnumerable<SelectListItem> DbServers { get; set; }

        public string DbServer { get; set; }

        public bool IsShowPlansJson { get; set; }

        public string ProductNo { get; set; }

        public int CurrentSegmentNo { get; set; }

        public List<FlightsPostModel> FlightSegmentPostModels { get; set; }

        public FlightSeat FlightSeat { get; set; }

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }
    }
}