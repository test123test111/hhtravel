using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace HHTravel.Infrastructure.Crosscutting
{
    /// <summary>
    /// 航段
    /// </summary>
    public class FlightsSegment
    {
        public FlightsSegment()
        {
            this.Flights = new List<Flight>();
        }

        public string SelectedRouteId { get; set; }

        public City DepartCity { get; set; }

        public City ArrivalCity { get; set; }

        public DateTime DepartDate { get; set; }

        public int SegmentNo { get; set; }

        public List<Flight> Flights { get; set; }

        public List<FlightsPlan> FlightsPlans { get; set; }

        public Airline Airline { get; set; }

        public int AdjustableDays { get; set; }

        [ContractInvariantMethod()]
        internal void Invariant()
        {
            Contract.Invariant(this.SegmentNo > 0);
            Contract.Invariant(!string.IsNullOrEmpty(this.DepartCity.CityName));
            Contract.Invariant(!string.IsNullOrEmpty(this.ArrivalCity.CityName));
        }
    }
}