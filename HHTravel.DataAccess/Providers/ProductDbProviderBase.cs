using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using HHTravel.DataAccess.DbContexts;

namespace HHTravel.DataAccess.Providers
{
    public abstract class ProductDbProviderBase<T> : IDataProvider<T> where T : class, new()
    {
        protected ProductDbProviderBase(ProductDbEntities productDbContext)
        {
            this.ProductDbContext = productDbContext;
        }

        protected ProductDbEntities ProductDbContext { get; private set; }

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

        public T Create()
        {
            return ProductDbContext.Set<T>().Create();
        }

        public void Delete(T entity)
        {
            ProductDbContext.Set<T>().Remove(entity);
            ProductDbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (this.ProductDbContext != null)
            {
                this.ProductDbContext.Dispose();
            }
        }

        public abstract T Find(params object[] keyValues);

        public T Insert(T entity)
        {
            ProductDbContext.Set<T>().Add(entity);
            ProductDbContext.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            //执行验证业务
            //context.Entry<T>(entity).GetValidationResult();
            if (ProductDbContext.Entry<T>(entity).State == EntityState.Modified)
                ProductDbContext.SaveChanges();
            return entity;
        }
    }
}