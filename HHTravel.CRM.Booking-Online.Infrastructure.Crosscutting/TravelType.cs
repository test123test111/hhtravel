using System.Runtime.Serialization;
using System;
using System.ComponentModel;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    /// <summary>
    /// 旅游型态
    /// </summary>
    [DataContract]
    public enum TravelType : short
    {
        /// <summary>
        /// 私家团(全程领队服务)
        /// </summary>
        [Description("私家团(全程领队服务)")]
        [DataMember]
        TravelType1 = 1,

        /// <summary>
        /// 私家团(当地专属车导)
        /// </summary>
        [Description("私家团(当地专属车导)")]
        [DataMember]
        TravelType4 = 4,

        /// <summary>
        /// 自由行
        /// </summary>
        [Description("自由行")]
        [DataMember]
        TravelType2 = 2,

        /// <summary>
        /// HH33
        /// </summary>
        [Description("HH33")]
        [DataMember]
        TravelType3 = 3,
    }
}