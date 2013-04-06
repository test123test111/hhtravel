using System.Collections.Generic;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class ProductListModel
    {
        public ProductListModel()
        {
            this.IsShowLinkOfOtherDepartureCity = true;
            this.ProductModelList = new List<ProductModel>();
            this.PagerModel = new PagerModel();
        }

        /// <summary>
        /// 用于选中出发城市
        /// </summary>
        public DepartureCity? DepartureCity { get; set; }

        /// <summary>
        /// 是否显示选择其他出发城市的链接 默认：显示
        /// </summary>
        public bool IsShowLinkOfOtherDepartureCity { get; set; }

        /// <summary>
        /// 用于设置排序箭头
        /// </summary>
        public bool? Descending { get; set; }

        public PagerModel PagerModel { get; set; }

        public List<ProductModel> ProductModelList { get; set; }

        /// <summary>
        /// 用于设置排序箭头
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        /// 用于选中筛选框
        /// </summary>
        public int? TravelType { get; set; }
    }
}