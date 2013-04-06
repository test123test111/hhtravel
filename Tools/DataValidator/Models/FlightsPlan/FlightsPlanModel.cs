using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataValidator.Models.FlightsPlan
{
    public class FlightsPlanModel
    {
        public List<FlightModel> FlightModels { get; set; }

        public decimal AdultCount { get; set; }

        public decimal AdultFuelSurcharges { get; set; }

        public decimal AdultPrice { get; set; }

        public decimal AdultTax { get; set; }

        public decimal ChildCount { get; set; }

        public decimal ChildFuelSurcharges { get; set; }

        public decimal ChildPrice { get; set; }

        public decimal ChildTax { get; set; }

        public string FlightsPlanJson { get; set; }

        public bool IsDirect { get; set; }

        public decimal TotalFuelSurcharges { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalTax { get; set; }
    }
}