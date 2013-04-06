using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataValidator.Models.FlightsPlan
{
    public class FlightsPostModel
    {
        public FlightsPostModel()
        {
            this.DepartDate = DateTime.Now;
        }

        public string DepartCityCode { get; set; }

        public string ArrivalCityCode { get; set; }

        [DataType(DataType.Date)]
        public DateTime DepartDate { get; set; }

        public int SegmentNo { get; set; }
    }
}