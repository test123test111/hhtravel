using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    public struct City
    {
        public string CityCode { get; set; }

        public string CityName { get; set; }

        public City(string cityCode, string cityName)
            : this()
        {
            CityCode = cityCode;
            CityName = cityName;
        }

        public static City Parse(string cityCode)
        {
            return new City
            {
                CityCode = cityCode,
            };
        }

        #region equals override

        // http://stackoverflow.com/questions/1502451/c-what-needs-to-be-overriden-in-a-struct-to-ensure-equality-operates-properly

        public override bool Equals(Object obj)
        {
            return obj is City && this == (City)obj;
        }

        public override int GetHashCode()
        {
            if (this.CityCode != null)
                return this.CityCode.GetHashCode();
            return 0;
        }

        public static bool operator ==(City x, City y)
        {
            return x.CityCode == y.CityCode;
        }

        public static bool operator !=(City x, City y)
        {
            return !(x == y);
        }

        #endregion equals override
    }
}