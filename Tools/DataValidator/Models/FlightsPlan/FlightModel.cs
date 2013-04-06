using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace DataValidator.Models.FlightsPlan
{
    public class FlightModel
    {
        public FlightModel()
        {
        }

        public string FlightNo { get; set; }

        public string ActualFlightNo { get; set; }

        public Airline Airline { get; set; }

        public Airline ActualAirline { get; set; }

        public Airport DepartAirport { get; set; }

        public City DepartCity { get; set; }

        public Airport ArrivalAirport { get; set; }

        public City ArrivalCity { get; set; }

        public string DepartTimeString { get; set; }

        public string ArrivalTimeString { get; set; }

        public int ArrivalDays { get; set; }

        [DisplayFormat(DataFormatString = "{0:H小时m分钟}")]
        public TimeSpan FlightDuration { get; set; }

        public Aircraft Aircraft { get; set; }

        public FlightSeat FlightClass { get; set; }

        public string Baggage { get; set; }
    }
}