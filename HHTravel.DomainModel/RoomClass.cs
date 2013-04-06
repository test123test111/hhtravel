using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HHTravel.DomainModel
{
    /// <summary>
    /// 房型信息
    /// </summary>
    [DataContract]
    public class RoomClass : IComparable<RoomClass>, IPriceDate
    {
        public RoomClass()
        {
            this.PriceDateList = new List<PriceDate>();
        }

        /// <summary>
        /// 早晚餐说明
        /// </summary>
        [DataMember]
        public string BreakfastDinnerTip { get; set; }

        /// <summary>
        /// 最大可住人数
        /// </summary>
        [DataMember]
        public int MaxOccupancy { get; set; }

        /// <summary>
        /// 获取 PriceDateList
        /// </summary>
        [DataMember]
        public List<PriceDate> PriceDateList { get; set; }

        /// <summary>
        /// 房型Id
        /// </summary>
        [DataMember]
        public int RoomClassId { get; set; }

        /// <summary>
        /// 房型名称
        /// </summary>
        [DataMember]
        public string RoomClassName { get; set; }

        /// <summary>
        /// Implement the generic CompareTo method with the Temperature
        /// class as the Type parameter.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(RoomClass other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null || !other.PriceDateList.Any()) return 1;
            if (!this.PriceDateList.Any()) return -1;

            // 价格最低的
            return this.PriceDateList.Min().CompareTo(other.PriceDateList.Min());
        }

        /// <summary>
        /// 获取PriceDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public PriceDate GetPriceDate(DateTime date)
        {
            PriceDate dDate = (from dd in this.PriceDateList
                               where dd.Date == date
                               select dd).SingleOrDefault();
            return dDate;
        }

        public decimal GetTotalPrice(DateTime beginDate, DateTime endDate)
        {
            decimal totalPrice = (from dd in this.PriceDateList
                                  where dd.Date < endDate && dd.Date >= beginDate
                                  select dd.Price).Sum();
            return totalPrice;
        }
    }
}