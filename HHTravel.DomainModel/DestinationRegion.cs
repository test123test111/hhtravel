using System.Runtime.Serialization;

namespace HHTravel.DomainModel
{
    /// <summary>
    /// 目的地所属区域
    /// </summary>
    [DataContract]
    public class DestinationRegion
    {
        /// <summary>
        /// 区域名称
        /// 例如：80天、英法日荷、加勒比海、南北极
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int RegionId { get; set; }
    }
}