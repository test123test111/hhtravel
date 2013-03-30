using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 旅游形态
    /// 团队游、自由行、单品游
    /// </summary>
    [DataContract]
    public enum TravelType : short
    {
        /// <summary>
        /// 团队游
        /// </summary>
        [DataMember]
        团队游 = 1,
        /// <summary>
        /// 自由行
        /// </summary>
        [DataMember]
        自由行 = 2,
        /// <summary>
        /// 单品游
        /// </summary>
        [DataMember]
        单品游 = 3,
    }
}
