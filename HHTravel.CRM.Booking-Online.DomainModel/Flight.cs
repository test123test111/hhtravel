using System;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    /// <summary>
    /// 航班信息
    /// </summary>
    [DataContract]
    public class Flight
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        [DataMember]
        public string Airline { get; set; }

        /// <summary>
        /// 到达机场
        /// </summary>
        [DataMember]
        public string ArrivalAirport { get; set; }

        /// <summary>
        /// 到达时间
        /// </summary>
        [DataMember]
        public TimeSpan? ArrivalTime { get; set; }

        /// <summary>
        /// 出发机场
        /// </summary>
        [DataMember]
        public string DepartureAirport { get; set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        [DataMember]
        public TimeSpan? DepartureTime { get; set; }

        /// <summary>
        /// 航班编号
        /// </summary>
        [DataMember]
        public string FlightNo { get; set; }

        /// <summary>
        /// 是否去程航班
        /// </summary>
        [DataMember]
        public bool IsGo { get; set; }
    }
}