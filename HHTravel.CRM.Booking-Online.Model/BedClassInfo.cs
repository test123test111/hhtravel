using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 床型信息
    /// </summary>
    [DataContract]
    public class BedClassInfo
    {
        /// <summary>
        /// 床型Id
        /// </summary>
        [DataMember]
        public string BedClassId { get; set; }
        /// <summary>
        /// 床型名称
        /// </summary>
        [DataMember]
        public string BedClassName { get; set; }
        /// <summary>
        /// 早晚餐说明
        /// </summary>
        [DataMember]
        public string BreakfastDinnerTip { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [DataMember]
        public int Price { get; set; }

        /// <summary>
        /// 生效日期
        /// </summary>
        [DataMember]
        public DateTime EffectDate { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        [DataMember]
        public DateTime ExpireDate { get; set; }
    }
}
