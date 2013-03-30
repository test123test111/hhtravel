using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 出发地
    /// 北京、上海、香港、成都、广州
    /// </summary>
    [DataContract]
    public class DepartureCity
    {
        [DataMember]
        public int DeparturePlaceId { get; set; }
        /// <summary>
        /// 北京、上海、香港、成都、广州
        /// </summary>
        [DataMember]
        public string Name { get; set; }
    }
}
