using System.Collections.Generic;
using HHTravel.DataAccess.DbContexts;
using HHTravel.Entity;

namespace HHTravel.DataAccess.Providers
{
    public class OrderProductsProvider : OrderDbProviderBase<Orders_Product>
    {
        public OrderProductsProvider()
            : base()
        {
        }

        public OrderProductsProvider(OrderDbEntities orderDbContext)
            : base(orderDbContext)
        {
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="orders_productList"></param>
        public virtual void Insert(IEnumerable<Orders_Product> orders_productList)
        {
            var dbSet = this.OrderDbContext.Set<Orders_Product>();
            foreach (var orders_product in orders_productList)
            {
                dbSet.Add(orders_product);
            }
            this.OrderDbContext.SaveChanges();
        }
    }
}