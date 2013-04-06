using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace HHTravel.Site.Models
{
    public class SearchPostModel
    {
        public DateTime? BeginDate { get; set; }

        public int? DaysSection { get; set; }

        public string DepartureCity { get; set; }

        public bool? Descending { get; set; }

        public int? DestinationGroupId { get; set; }

        public int? DestinationRegionId { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Interestid { get; set; }

        public int? PageIndex { get; set; }

        /// <summary>
        /// 按产品名称模糊搜索
        /// </summary>
        public string ProductName { get; set; }

        public int? Sort { get; set; }

        public int? TravelType { get; set; }
    }
}