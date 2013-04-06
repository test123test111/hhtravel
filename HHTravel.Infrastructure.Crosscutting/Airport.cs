using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.Infrastructure.Crosscutting
{
    /// <summary>
    /// 机场
    /// </summary>
    public struct Airport
    {
        public string AirportCode { get; set; }

        public string AirportName { get; set; }

        public static Airport Parse(string airportCode)
        {
            return new Airport
            {
                AirportCode = airportCode,
            };
        }

        #region equals override

        // http://stackoverflow.com/questions/1502451/c-what-needs-to-be-overriden-in-a-struct-to-ensure-equality-operates-properly

        public override bool Equals(Object obj)
        {
            return obj is Airport && this == (Airport)obj;
        }

        public override int GetHashCode()
        {
            return this.AirportCode.GetHashCode();
        }

        public static bool operator ==(Airport x, Airport y)
        {
            return x.AirportCode == y.AirportCode;
        }

        public static bool operator !=(Airport x, Airport y)
        {
            return !(x == y);
        }

        #endregion equals override
    }
}