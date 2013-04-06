using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    /// <summary>
    /// 有价格的日期
    /// </summary>
    public class PriceDate : IComparable<PriceDate>
    {
        /// <summary>
        /// 儿童价
        /// </summary>
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ChildPrice { get; set; }

        /// <summary>
        /// 儿童延住价格（按日）
        /// </summary>
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ChildStayPricePerDay { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 最小成团人数
        /// </summary>
        public short MinGroupNum { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Price { get; set; }

        /// <summary>
        /// 产品基础价
        /// </summary>
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ProductBasicPrice { get; set; }

        /// <summary>
        /// 延住价格（按日）
        /// </summary>
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal StayPricePerDay { get; set; }

        /// <summary>
        /// Implement the generic CompareTo method with the Temperature
        /// class as the Type parameter.</summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(PriceDate other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;
            return this.Price.CompareTo(other.Price);
        }
    }
}