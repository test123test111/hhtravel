using System;
using System.Runtime.Serialization;

namespace HHTravel.DomainModel
{
    public class OrderItem
    {
        public OrderItem(string productType, int? productId, string productName, int? specId, string specName,
            int quantity, decimal unitPrice, DateTime? beginDate, DateTime? endDate, string unit)
        {
            this.ProductType = productType;
            this.ProductId = productId;
            this.ProductName = productName;
            this.SpecId = specId;
            this.SpecName = specName;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
            this.BeginDate = beginDate;
            this.EndDate = endDate;
            this.Unit = unit;
            this.Times = 1; // 默认值
        }

        /// <summary>
        /// 数量
        /// </summary>
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// 单价（最小的可计量单位）
        /// 对于考虑变价的酒店产品，单价为，一间房间X天每天价格的累计；
        /// 对于TravelType1/4的酒店产品，单价为，一个人X天的住宿价格；
        /// </summary>
        [DataMember]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [DataMember]
        public decimal Amount { get { return this.UnitPrice * this.Quantity; } }

        /// <summary>
        /// 开始日期
        /// </summary>
        [DataMember]
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 频次
        /// </summary>
        [DataMember]
        public int Times { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 规格Id
        /// </summary>
        [DataMember]
        public int? SpecId { get; set; }

        /// <summary>
        /// 酒店、航班、加购机票去、加购机票回、酒店延住、地面产品
        /// </summary>
        [DataMember]
        public int? ProductId { get; set; }

        [DataMember]
        public string ProductName { get; set; }

        [DataMember]
        public string SpecName { get; set; }

        [DataMember]
        public string ProductType { get; set; }

        [DataMember]
        public string Unit { get; set; }

        [DataMember]
        public string Remark { get; set; }
    }
}