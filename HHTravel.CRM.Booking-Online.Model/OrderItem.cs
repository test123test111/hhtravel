using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    public class OrderItem
    {
        /// <summary>
        /// 酒店、航班、加够机票去、加购机票回、酒店延住
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }
        /// <summary>
        /// 规格Id
        /// </summary>
        [DataMember]
        public int SpecId { get; set; }

        /// <summary>
        /// 参加人数（成人）
        /// </summary>
        [DataMember]
        public short? AdultNum { get; set; }
        /// <summary>
        /// 参加人数（儿童）
        /// </summary>
        [DataMember]
        public short? ChildNum { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public short? Amount { get; set; }

        /// <summary>
        /// 出发日期
        /// </summary>
        [DataMember]
        public DateTime DepartureDate { get; set; }
        /// <summary>
        /// （酒店）回程日期？
        /// </summary>
        [DataMember]
        public DateTime? ReturnDate { get; set; }
        /// <summary>
        /// 成人价格
        /// </summary>
        [DataMember]
        public int? AdultPrice { get; set; }
        /// <summary>
        /// 儿童价格
        /// </summary>
        [DataMember]
        public int? ChildPrice { get; set; }

        /// <summary>
        /// （酒店）是否延住
        /// </summary>
        [DataMember]
        public bool? IsHotelStay { get; set; }
        /// <summary>
        /// （酒店）天数
        /// </summary>
        [DataMember]
        public short? HotelDays { get; set; }
    }
}
