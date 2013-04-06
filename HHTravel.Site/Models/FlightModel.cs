using System;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class FlightModel
    {
        /// <summary>
        /// 航空公司
        /// </summary>
        public string Airline { get; set; }

        /// <summary>
        /// 到达机场
        /// </summary>
        public string ArrivalAirport { get; set; }

        /// <summary>
        /// 到达时间
        /// 例如15:50
        /// </summary>
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan? ArrivalTime { get; set; }

        /// <summary>
        /// 出发机场
        /// </summary>
        public string DepartureAirport { get; set; }

        /// <summary>
        /// 出发时间
        /// 例如09:20
        /// </summary>
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan? DepartureTime { get; set; }

        /// <summary>
        /// 航班编号
        /// </summary>
        public string FlightNo { get; set; }

        /// <summary>
        /// 行程段 去程、回程、中间程
        /// </summary>
        public string SectionName { get; set; }
    }
}