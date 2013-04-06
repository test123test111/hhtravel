using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    [DataContract]
    public class Pager
    {
        public Pager()
        {
            this.PageIndex = 0;
            this.PageSize = 5;
        }

        public Pager(int? pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public Pager(int? sort, bool? descending, int? pageIndex, int pageSize)
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
                this.SortRule = SortRule.ProductDefault;
                this.Descending = true;
            }

            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public Pager(SortRule sortRule, bool descending, int? pageIndex, int pageSize)
        {
            this.SortRule = sortRule;
            this.Descending = descending;
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        [DataMember]
        public bool Descending { get; private set; }

        [DataMember]
        public int PageCount { get; set; }

        [DataMember]
        public int? PageIndex { get; private set; }

        [DataMember]
        public int PageSize { get; private set; }

        [DataMember]
        public SortRule SortRule { get; private set; }
    }
}