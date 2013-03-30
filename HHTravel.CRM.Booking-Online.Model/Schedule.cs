using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    [DataContract]
    public class ScheduleItem
    {
        [DataMember]
        public int Sort { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public ScheduleItemInfos Infos { get; set; }
    }

    public class ScheduleItemInfos
    {
        /// <summary>
        /// 航班
        /// </summary>
        [DataMember]
        public string FlightInfo { get; set; }
        /// <summary>
        /// 酒店
        /// </summary>
        [DataMember]
        public string HotelInfo { get; set; }
        /// <summary>
        /// 餐饮
        /// </summary>
        [DataMember]
        public string CateringInfo { get; set; }
        /// <summary>
        /// 景点
        /// </summary>
        [DataMember]
        public string SpotsInfo { get; set; }
    }
}
