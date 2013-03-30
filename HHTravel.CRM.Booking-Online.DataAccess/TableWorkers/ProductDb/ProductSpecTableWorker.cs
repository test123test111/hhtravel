using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using System.Data.Entity;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    internal class ProductSpecTableWorker : ProductDbTableWorkerBase<Product_Spec>
    {
        public ProductSpecTableWorker()
            : base(DbContextFactory.Create<ProductDbEntities>())
        {

        }

        public ProductSpecTableWorker(ProductDbEntities productDbContext)
            : base(productDbContext)
        {
        }

        /// <summary>
        /// 获取有效的Product_Spec
        /// </summary>
        /// <returns></returns>
        public override IQueryable<Product_Spec> All()
        {
            var a = from s in ProductDbContext.Product_Spec
                    where s.IsValid.HasValue && s.IsValid.Value
                    select s;
            return a;
        }

        public override Product_Spec Find(int id)
        {
            throw new NotImplementedException();
        }
    }
}
