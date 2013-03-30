using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    internal class OrdersProductTableWorker : OrderDbTableWorkerBase<Orders_Product>
    {
        public OrdersProductTableWorker()
            : base()
        {

        }

        public OrdersProductTableWorker(OrderDbEntities orderDbContext)
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
