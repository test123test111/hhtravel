using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 旅行主题
    /// </summary>
    [DataContract]
    public class Interest : IAggregateRoot
    {
        [DataMember]
        public int InterestId { get; set; }
        /// <summary>
        /// 主题名称
        /// 例如：地球标记、高端火车、高端自由行
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
