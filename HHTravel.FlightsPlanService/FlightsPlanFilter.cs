using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.FlightsPlanService
{
    public class FlightsPlanFilter
    {
        public FlightsPlanFilter()
        {
            this.IncludeAirlineCodes = new List<string>();
            this.ExcludeAirlineCodes = new List<string>();
            this.IncludeFlightNos = new List<string>();
            this.ExcludeFlightNos = new List<string>();
        }

        private static TimeSpan s_defaultEarliestTime = TimeSpan.Parse("00:00");
        private static TimeSpan s_defaultLatestTime = TimeSpan.Parse("23:59");

        /// <summary>
        /// 航程编号
        /// </summary>
        public int FlightSegmentNo { get; set; }

        public TimeSpan EarliestTime { get; set; }

        public TimeSpan LatestTime { get; set; }

        /// <summary>
        /// 航空公司两字码 空=全部
        /// </summary>
        public List<string> IncludeAirlineCodes { get; set; }

        public List<string> ExcludeAirlineCodes { get; set; }

        public List<string> IncludeFlightNos { get; set; }

        public List<string> ExcludeFlightNos { get; set; }

        public bool IsDirect { get; set; }

        public bool CheckDirect(int flightsCount)
        {
            return (!this.IsDirect || (this.IsDirect && flightsCount < 2));
        }

        public bool CheckAriline(string airlineCode)
        {
            if (string.IsNullOrEmpty(airlineCode))
            {
                throw new ArgumentNullException("airlineCode");
            }

            bool pass = true;
            if (this.IncludeAirlineCodes.Count > 0)
            {
                pass = this.IncludeAirlineCodes.Contains(airlineCode);
            }

            if (this.ExcludeAirlineCodes.Count > 0)
            {
                pass = !this.ExcludeAirlineCodes.Contains(airlineCode);
            }

            return pass;
        }

        public bool CheckFlightNo(string flightNo)
        {
            if (string.IsNullOrEmpty(flightNo))
            {
                throw new ArgumentNullException("flightNo");
            }

            bool pass = true;
            if (this.IncludeFlightNos.Count > 0)
            {
                pass = this.IncludeFlightNos.Contains(flightNo);
            }

            if (this.ExcludeFlightNos.Count > 0)
            {
                pass = !this.ExcludeFlightNos.Contains(flightNo);
            }

            return pass;
        }

        public static FlightsPlanFilter Default
        {
            get
            {
                return new FlightsPlanFilter
                {
                    EarliestTime = s_defaultEarliestTime,
                    LatestTime = s_defaultLatestTime,
                    IsDirect = true,
                };
            }
        }
    }
}