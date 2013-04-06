using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using HHTravel.DataAccess.DbContexts;

namespace HHTravel.DataAccess.Providers
{
    public abstract class OrderDbProviderBase<T> : IDataProvider<T> where T : class, new()
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected OrderDbProviderBase()
        {
            this.OrderDbContext = DbContextFactory.Create<OrderDbEntities>();
        }

        protected OrderDbProviderBase(OrderDbEntities orderDbContext)
        {
            this.OrderDbContext = orderDbContext;
        }

        protected OrderDbEntities OrderDbContext { get; set; }

        public virtual IQueryable<T> All()
        {
            return OrderDbContext.Set<T>();
        }

        public virtual IQueryable<T> AllIncluding(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = OrderDbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Create()
        {
            return OrderDbContext.Set<T>().Create();
        }

        public void Delete(T entity)
        {
            OrderDbContext.Set<T>().Remove(entity);
            OrderDbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (this.OrderDbContext != null)
            {
                this.OrderDbContext.Dispose();
            }
        }

        public T Find(params object[] keyValues)
        {
            var entity = OrderDbContext.Set<T>().Find(keyValues);
            return entity;
        }

        public T Insert(T entity)
        {
            OrderDbContext.Set<T>().Add(entity);
            OrderDbContext.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            //执行验证业务
            //context.Entry<T>(entity).GetValidationResult();
            //if (OrderDbContext.Entry<T>(entity).State == EntityState.Modified)
            OrderDbContext.Entry<T>(entity).State = EntityState.Modified;
            OrderDbContext.SaveChanges();
            return entity;
        }
    }
}