using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DataValidator.Models
{
    public class SearchBoxModel
    {
        public static IEnumerable<SelectListItem> DaysSections { get; set; }

        public static IEnumerable<SelectListItem> DepartureCitys { get; set; }

        public static IEnumerable<SelectListItem> DestinationGroups { get; set; }

        public static IEnumerable<SelectListItem> Interests { get; set; }

        public static IEnumerable<SelectListItem> TravelTypes { get; set; }

        public static IEnumerable<SelectListItem> DbServers { get; set; }

        public string DaysSection { get; set; }

        public DateTime DepartureBeginDate { get; set; }

        public string DepartureCity { get; set; }

        public DateTime DepartureEndDate { get; set; }

        public string DestinationGroup { get; set; }

        public string DestinationRegion { get; set; }

        public string Interest { get; set; }

        public string TravelType { get; set; }

        public string DbServer { get; set; }
    }
}