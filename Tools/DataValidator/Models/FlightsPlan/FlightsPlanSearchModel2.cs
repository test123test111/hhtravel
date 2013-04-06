using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataValidator.Models.FlightsPlan
{
    public class FlightsPlanSearchModel2
    {
        public string DbServer { get; set; }

        public bool IsShowPlansJson { get; set; }

        public string SearchConditionJson { get; set; }

        public string FlightsPlanJson { get; set; }
    }
}