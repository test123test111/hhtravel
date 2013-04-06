using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    /// <summary>
    /// 目的地所属分组
    /// </summary>
    [DataContract]
    public class DestinationGroup : IAggregateRoot
    {
        private List<DestinationRegion> _regionList = new List<DestinationRegion>();

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
        public List<DestinationRegion> RegionList { get { return _regionList; } set { _regionList = value; } }
    }
}