using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    /// <summary>
    /// 航空公司
    /// </summary>
    public struct Airline
    {
        public string AirlineName { get; set; }

        public string AirlineCode { get; set; }

        public static Airline Parse(string airlineCode)
        {
            return new Airline
            {
                AirlineCode = airlineCode,
            };
        }

        #region equals override

        // http://stackoverflow.com/questions/1502451/c-what-needs-to-be-overriden-in-a-struct-to-ensure-equality-operates-properly

        public override bool Equals(Object obj)
        {
            return obj is Airline && this == (Airline)obj;
        }

        public override int GetHashCode()
        {
            return this.AirlineCode.GetHashCode();
        }

        public static bool operator ==(Airline x, Airline y)
        {
            return x.AirlineCode == y.AirlineCode;
        }

        public static bool operator !=(Airline x, Airline y)
        {
            return !(x == y);
        }

        #endregion equals override
    }
}