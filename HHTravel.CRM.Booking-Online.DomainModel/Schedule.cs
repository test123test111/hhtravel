using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    /// <summary>
    /// 日程项
    /// </summary>
    [DataContract]
    public class ScheduleItem
    {
        [DataMember]
        public ScheduleItemInfos Infos { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Sort { get; set; }
    }

    public class ScheduleItemInfos
    {
        /// <summary>
        /// 餐饮
        /// </summary>
        [DataMember]
        public string CateringInfo { get; set; }

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
        /// 景点
        /// </summary>
        [DataMember]
        public string SpotsInfo { get; set; }
    }
}