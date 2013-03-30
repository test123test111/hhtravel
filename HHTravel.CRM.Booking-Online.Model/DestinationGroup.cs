using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 目的地所属分组
    /// </summary>
    [DataContract]
    public class DestinationGroup : IAggregateRoot
    {
        [DataMember]
        public int GroupId { get; set; }
        /// <summary>
        /// 例如：环游世界、欧洲、日韩港澳台、顶级游轮
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// 包含的区域列表
        /// </summary>
        [DataMember]
        public List<DestinationRegion> RegionList { get; set; }
        /// <summary>
        /// 营销图
        /// </summary>
        [DataMember]
        public ImageInfo TopImage { get; set; }
    }
}
