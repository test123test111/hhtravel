using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 房型信息
    /// 自由行 只包含双人房价格、不指明床型、延住不指明价格、是否显示席次
    /// 团队游 单双房价格分列、不指明床型、延住不指明价格、是否显示席次
    /// 单品游 单双房价格分列、指明床型(床型价格分列)、延住指明价格（按床型价格分列）、是否显示席次
    /// </summary>
    [DataContract]
    public class RoomClassInfo
    {
        /// <summary>
        /// 床型信息
        /// </summary>
        [DataMember]
        public List<BedClassInfo> BedClassInfos { get; set; }

        #region 单品游专用
        /// <summary>
        /// 入住日期
        /// </summary>
        [DataMember]
        public DateTime CheckInDate { get; set; }
        /// <summary>
        /// 退房日期
        /// </summary>
        [DataMember]
        public DateTime CheckOutDate { get; set; }

        /// <summary>
        /// 双人房大床价格
        /// </summary>
        [DataMember]
        public int DoubleRoomOneBedsPrice { get; set; }
        [DataMember]
        public string DoubleRoomOneBedsTips { get; set; }
        /// <summary>
        /// 双人房大床延住价格
        /// </summary>
        [DataMember]
        public int DoubleRoomOneBedsRenewPrice { get; set; }
        /// <summary>
        /// 双人房双床价格
        /// </summary>
        [DataMember]
        public int DoubleRoomTwoBedsPrice { get; set; }
        [DataMember]
        public string DoubleRoomTwoBedsTips { get; set; }
        /// <summary>
        /// 双人房双床延住价格
        /// </summary>
        [DataMember]
        public int DoubleRoomTwoBedsRenewPrice { get; set; }

        /// <summary>
        /// 单人房标准床价格
        /// </summary>
        [DataMember]
        public int SingleRoomOneBedsPrice { get; set; }
        [DataMember]
        public string SingleRoomOneBedsTips { get; set; }
        /// <summary>
        /// 单人房标准床延住价格
        /// </summary>
        [DataMember]
        public int SingleRoomOneBedsRenewPrice { get; set; }
        /// <summary>
        /// 单人房大床价格
        /// </summary>
        [DataMember]
        public int SingleRoomLargerBedPrice { get; set; }
        [DataMember]
        public string SingleRoomStandardBedTips { get; set; }
        /// <summary>
        /// 单人房大床延住价格
        /// </summary>
        [DataMember]
        public int SingleRoomLargerBedRenewPrice { get; set; } 
        #endregion


    }


}
