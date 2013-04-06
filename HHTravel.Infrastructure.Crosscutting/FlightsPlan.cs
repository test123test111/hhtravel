using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace HHTravel.Infrastructure.Crosscutting
{
    /// <summary>
    /// 航班计划
    /// </summary>
    public class FlightsPlan
    {
        public string FareId { get; set; }

        public Airline Airline { get; set; }

        public List<Flight> Flights { get; set; }

        public string RouteId { get; set; }

        #region 成人票的价格和税费

        public decimal AdultFuelSurcharges { get; set; }

        public decimal AdultTax { get; set; }

        public decimal AdultPrice { get; set; }

        #endregion 成人票的价格和税费

        #region 儿童票的价格和税费

        public decimal ChildFuelSurcharges { get; set; }

        public decimal ChildTax { get; set; }

        public decimal ChildPrice { get; set; }

        #endregion 儿童票的价格和税费

        public int AdultCount { get; set; }

        public int ChildCount { get; set; }

        public decimal TotalFuelSurcharges { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalTax { get; set; }

        public override bool Equals(object obj)
        {
            var x = this;
            var y = obj as FlightsPlan;

            if (y == null)
            {
                return false;
            }

            if (x.Flights.Count != y.Flights.Count)
            {
                return false;
            }

            for (int i = 0; i < x.Flights.Count; i++)
            {
                var fx = x.Flights[i];
                var fy = y.Flights[i];

                if (fx.FlightNo != fy.FlightNo || fx.FlightSeat != fy.FlightSeat)
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.Flights.Count.GetHashCode();
        }
    }
}