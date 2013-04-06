using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    /// <summary>
    /// 灰机
    /// </summary>
    public struct Aircraft
    {
        public string Model { get; set; }

        public string Type { get; set; }

        public int MinSeats { get; set; }

        public int MaxSeats { get; set; }

        public static Aircraft Parse(string model)
        {
            return new Aircraft
            {
                Model = model,
            };
        }

        #region equals override

        // http://stackoverflow.com/questions/1502451/c-what-needs-to-be-overriden-in-a-struct-to-ensure-equality-operates-properly

        public override bool Equals(Object obj)
        {
            return obj is Aircraft && this == (Aircraft)obj;
        }

        public override int GetHashCode()
        {
            return this.Model.GetHashCode();
        }

        public static bool operator ==(Aircraft x, Aircraft y)
        {
            return x.Model == y.Model;
        }

        public static bool operator !=(Aircraft x, Aircraft y)
        {
            return !(x == y);
        }

        #endregion equals override
    }
}