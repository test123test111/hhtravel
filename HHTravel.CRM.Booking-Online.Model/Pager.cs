using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    [DataContract]
    public class Pager
    {
        [DataMember]
        public SortRule SortRule { get; private set; }

        [DataMember]
        public bool Descending { get; private set; }

        [DataMember]
        public int? PageIndex { get; private set; }

        [DataMember]
        public int PageCount { get; set; }

        public Pager()
        {
            this.PageIndex = 0;
        }

        public Pager(int? pageIndex)
        {
            this.PageIndex = pageIndex;
        }

        public Pager(int? sort, bool? descending, int? pageIndex)
        {
            //todo: 验证sort是否在定义中

            if (sort.HasValue)
            {
                this.SortRule = (SortRule)sort;
                this.Descending = descending ?? false;
            }
            else
            {
                // 默认排序规则
                this.SortRule = SortRule.ProductWeight;
                this.Descending = true;
            }

            this.PageIndex = pageIndex;
        }

        public Pager(SortRule sortRule, bool descending, int? pageIndex)
        {
            this.SortRule = sortRule;
            this.Descending = descending;
            this.PageIndex = pageIndex;
        }
    }

    [DataContract]
    public enum SortRule
    {
        /// <summary>
        /// 按行程天数
        /// </summary>
        [DataMember]
        ProductTripDays = 1,
        /// <summary>
        /// 按最早出发日期
        /// </summary>
        [DataMember]
        ProductMinDepartureDate = 2,
        /// <summary>
        /// 按最低价格
        /// </summary>
        [DataMember]
        ProductMinPrice = 3,
        /// <summary>
        /// 默认排序规则: 推荐等级(降)+出发日期(升)+天数(升)
        /// </summary>
        [DataMember]
        ProductDefault = 0,
    }
}
