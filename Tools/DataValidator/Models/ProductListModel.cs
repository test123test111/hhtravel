using System;
using System.Collections.Generic;

namespace DataValidator.Models
{
    public class ProductListModel
    {
        public static int PageSize = 500;

        public ProductListModel()
        {
            this.ProductList = new List<ProductModel>();

            var now = DateTime.Now.Date;
            var endDate = now.AddMonths(1);
            this.SearchBox = new SearchBoxModel
            {
                DepartureBeginDate = now,
                DepartureEndDate = endDate,
            };
        }

        public string DomainBase { get; set; }

        public IList<ProductModel> ProductList { get; private set; }

        public SearchBoxModel SearchBox { get; private set; }
    }
}