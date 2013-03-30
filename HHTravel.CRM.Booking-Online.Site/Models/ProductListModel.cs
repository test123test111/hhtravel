using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class ProductListModel
    {
        public IEnumerable<Product> ProductList { get; set; }
        /// <summary>
        /// 用于选中筛选框
        /// </summary>
        public int? TravelType { get; set; }
        /// <summary>
        /// 用于设置排序箭头
        /// </summary>
        public int? Sort { get; set; }
        /// <summary>
        /// 用于设置排序箭头
        /// </summary>
        public bool? Descending { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}