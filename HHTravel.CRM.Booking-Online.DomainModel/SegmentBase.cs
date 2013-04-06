using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    [DataContract]
    public class SegmentBase : IPriceDate
    {
        public SegmentBase()
        {
            this.PriceDateList = new List<PriceDate>();
        }

        [DataMember]
        public List<PriceDate> PriceDateList { get; set; }

        [DataMember]
        public int SegmentId { get; set; }

        /// <summary>
        /// 获取PriceDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public PriceDate GetPriceDate(DateTime date)
        {
            var priceDate = (from dd in this.PriceDateList
                             where dd.Date == date
                             orderby dd.Price
                             select dd).FirstOrDefault();

            if (priceDate == null)
            {
                throw new ArgumentOutOfRangeException("date", date, "指定的日期该行程段缺少最低价格");
            }

            return priceDate;
        }
    }
}