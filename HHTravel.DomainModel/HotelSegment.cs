using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.DomainModel
{
    /// <summary>
    /// 行程段
    /// </summary>
    [DataContract]
    public class HotelSegment : SegmentBase
    {
        [DataMember]
        public int Nights { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public List<Hotel> HotelList { get; set; }

        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 酒店段的类型
        /// </summary>
        [DataMember]
        public HotelSegmentType HotelSegmentType { get; set; }
    }

}