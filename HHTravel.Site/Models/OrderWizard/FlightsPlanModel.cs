using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HHTravel.Infrastructure;
using Newtonsoft.Json;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models.OrderWizard
{
    public class FlightsPlanModel
    {
        public string RouteId { get; set; }

        public Airline Airline { get; set; }

        public Airport DepartAirport { get; set; }

        public Airport ArrivalAirport { get; set; }

        public string FlightNo { get; set; }

        public FlightSeat FlightSeat { get; set; }

        public string DepartTime { get; set; }

        public string ArrivalTime { get; set; }

        #region 成人票的价格和税费

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal AdultFuelSurcharges { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal AdultTax { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal AdultPrice { get; set; }

        #endregion 成人票的价格和税费

        #region 儿童票的价格和税费

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ChildFuelSurcharges { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ChildTax { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ChildPrice { get; set; }

        #endregion 儿童票的价格和税费

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalFuelSurcharges { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalTax { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalPriceDelta { get; set; }

        public List<FlightModel> Flights { get; set; }

        public string FlightSeatName { get { return this.FlightSeat.ToString(); } }

        public string FlightsSegmentPlanJson { get; set; }
    }

    public class FlightModel
    {
        public string FlightNo { get; set; }

        public Airline Airline { get; set; }

        public Airline ActualAirline { get; set; }

        public string ActualFlightNo { get; set; }

        public City DepartCity { get; set; }

        public City ArrivalCity { get; set; }

        public Aircraft Aircraft { get; set; }

        public TimeSpan DepartTime { get; set; }

        public TimeSpan ArrivalTime { get; set; }

        public Airport DepartAirport { get; set; }

        public Airport ArrivalAirport { get; set; }

        /// <summary>
        /// 到达天数
        /// </summary>
        public int ArrivalDays { get; set; }

        public string Baggage { get; set; }

        public FlightSeat FlightSeat { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public List<StopoverModel> Stopovers { get; set; }
    }

    public class StopoverModel
    {
        public string Airport { get; set; }

        public string Duration { get; set; }
    }
}