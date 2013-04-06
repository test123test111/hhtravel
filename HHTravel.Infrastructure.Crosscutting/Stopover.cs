using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    /// <summary>
    /// 经停信息
    /// </summary>
    public class Stopover
    {
        public string Airport { get; set; }

        public TimeSpan Duration { get; set; }
    }
}