using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.Infrastructure.Crosscutting
{
    /// <summary>
    /// 航班
    /// </summary>
    public class Flight
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

        public List<Stopover> Stopovers { get; set; }

        public TimeSpan FlightDuration { get; set; }

        /// <summary>
        /// 是否去程
        /// </summary>
        public bool IsGo { get; set; }
    }
}