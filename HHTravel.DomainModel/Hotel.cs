using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    /// <summary>
    /// 酒店信息
    /// </summary>
    [DataContract]
    public class Hotel
    {
        /// <summary>
        /// 酒店介绍
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 酒店亮点
        /// </summary>
        [DataMember]
        public string Feature { get; set; }

        /// <summary>
        /// 酒店Id
        /// </summary>
        [DataMember]
        public int HotelId { get; set; }

        /// <summary>
        /// 酒店名称
        /// </summary>
        [DataMember]
        public string HotelName { get; set; }

        /// <summary>
        /// 酒店图片
        /// </summary>
        [DataMember]
        public List<ImageInfo> PhotoList { get; set; }

        /// <summary>
        /// 房型
        /// </summary>
        [DataMember]
        public List<RoomClass> RoomClassList { get; set; }

        /// <summary>
        /// 官网地址
        /// </summary>
        [DataMember]
        public string Url { get; set; }

        /// <summary>
        /// 最大可住人数
        /// </summary>
        [DataMember]
        public int MaxOccupancy { get; set; }
    }
}