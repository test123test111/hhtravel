using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    /// <summary>
    /// 机票
    /// </summary>
    [DataContract]
    public class Ticket
    {
        /// <summary>
        /// ProductId
        /// </summary>
        [DataMember]
        public int TicketId { get; set; }
        /// <summary>
        /// 舱等Id
        /// todo: 从业务角度没必要，为了填充目前的订单子表中的SpecId字段
        /// </summary>
        [DataMember]
        public int? FlightClassId { get; set; }
        /// <summary>
        /// 舱等名称
        /// 例如：头等舱、公务舱
        /// </summary>
        [DataMember]
        public string FlightClassName { get; set; }
        /// <summary>
        /// 航班
        /// </summary>
        [DataMember]
        public List<Flight> FlightList { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        [DataMember]
        public int? Price { get; set; }
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
        /// <summary>
        /// 加购说明
        /// </summary>
        [DataMember]
        public string AdditionalPurchasesNote { get; set; }
    }
}
