using System;
using System.Collections.Generic;

namespace HHTravel.Entity
{
    public class ProductWrapper
    {
        public ICollection<Product_NoDeparture> ClosedDepartureDates { get; set; }

        public DateTime? FirstDepartureDate { get; set; }

        public DateTime LastDepartureDate { get; set; }

        /// <summary>
        /// （辅助计算的临时状态）
        /// </summary>
        public int MinPrice { get; set; }

        public ICollection<Product_Price> Prices { get; set; }

        public IEnumerable<Product_Price_Cache> PricesCache { get; set; }

        public Product Product { get; set; }
    }
}