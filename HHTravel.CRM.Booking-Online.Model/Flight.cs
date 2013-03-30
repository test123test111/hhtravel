using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 航班信息
    /// </summary>
    [DataContract]
    public class Flight
    {
        /// <summary>
        /// 是否去程航班
        /// </summary>
        [UIHint("_FlightIsGo")]
        [DataMember]
        public bool IsGo { get; set; }
        /// <summary>
        /// 航空公司
        /// </summary>
        [DataMember]
        public string Airline { get; set; }
        /// <summary>
        /// 航班编号
        /// </summary>
        [DataMember]
        public string FlightNo { get; set; }
        /// <summary>
        /// 出发机场
        /// </summary>
        [DataMember]
        public string DepartureAirport { get; set; }
        /// <summary>
        /// 到达机场
        /// </summary>
        [DataMember]
        public string ArrivalAirport { get; set; }
        /// <summary>
        /// 出发时间
        /// 例如09:20
        /// </summary>
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        [DataMember]
        public TimeSpan? DepartureTime { get; set; }
        /// <summary>
        /// 到达时间
        /// 例如15:50
        /// </summary>
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        [DataMember]
        public TimeSpan? ArrivalTime { get; set; }
    }
}
