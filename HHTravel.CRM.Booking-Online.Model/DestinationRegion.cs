using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 目的地所属区域
    /// </summary>
    [DataContract]
    public class DestinationRegion
    {
        [DataMember]
        public int RegionId { get; set; }
        /// <summary>
        /// 区域名称
        /// 例如：80天、英法日荷、加勒比海、南北极
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 营销图
        /// </summary>
        [DataMember]
        public ImageInfo TopImage { get; set; }
    }
}
