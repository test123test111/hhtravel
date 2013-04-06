using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    /// <summary>
    /// 旅游型态二级分类
    /// </summary>
    [DataContract]
    public enum TravelChildType
    {
        /// <summary>
        /// 小团旅
        /// </summary>
        [DataMember]
        无二级分类 = 0,

        /// <summary>
        /// 定点
        /// </summary>
        [DataMember]
        定点 = 1,

        /// <summary>
        /// 线路
        /// </summary>
        [DataMember]
        线路 = 2,
    }
}