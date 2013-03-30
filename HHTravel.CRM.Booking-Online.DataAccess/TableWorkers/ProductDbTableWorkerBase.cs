using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    public abstract class ProductDbTableWorkerBase<T> : ITableWorker<T> where T : class, new()
    {
        protected ProductDbEntities ProductDbContext { get; set; }

        public ProductDbTableWorkerBase(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        public abstract T Find(int id);

        public T Create()
        {
            return ProductDbContext.Set<T>().Create();
        }

        public T Update(T entity)
        {
            //执行验证业务
            //context.Entry<T>(entity).GetValidationResult();
            if (ProductDbContext.Entry<T>(entity).State == EntityState.Modified)
                ProductDbContext.SaveChanges();
            return entity;
        }

        public T Insert(T entity)
        {
            ProductDbContext.Set<T>().Add(entity);
            ProductDbContext.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            ProductDbContext.Set<T>().Remove(entity);
            ProductDbContext.SaveChanges();
        }

        public virtual IQueryable<T> All()
        {
            return ProductDbContext.Set<T>();
        }

        public virtual IQueryable<T> AllIncluding(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = ProductDbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }
    }
}
