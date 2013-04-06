using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using HHTravel.DataAccess.DbContexts;

namespace HHTravel.DataAccess.Providers
{
    public abstract class CustomerDbProviderBase<T> : IDataProvider<T> where T : class, new()
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        protected CustomerDbProviderBase()
        {
            this.CustomerDbContext = DbContextFactory.Create<CustomerDbEntities>();
        }

        protected CustomerDbProviderBase(CustomerDbEntities customerDbContext)
        {
            this.CustomerDbContext = customerDbContext;
        }

        protected CustomerDbEntities CustomerDbContext { get; set; }

        public virtual IQueryable<T> All()
        {
            return CustomerDbContext.Set<T>();
        }

        public virtual IQueryable<T> AllIncluding(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = CustomerDbContext.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public T Create()
        {
            return CustomerDbContext.Set<T>().Create();
        }

        public void Delete(T entity)
        {
            CustomerDbContext.Set<T>().Remove(entity);
            CustomerDbContext.SaveChanges();
        }

        public void Dispose()
        {
            if (this.CustomerDbContext != null)
            {
                this.CustomerDbContext.Dispose();
            }
        }

        public T Find(params object[] keyValues)
        {
            T entity = CustomerDbContext.Set<T>().Find(keyValues);
            return entity;
        }

        public T Insert(T entity)
        {
            CustomerDbContext.Configuration.ValidateOnSaveEnabled = false;
            CustomerDbContext.Set<T>().Add(entity);
            CustomerDbContext.SaveChanges();
            return entity;
        }

        public T Update(T entity)
        {
            //执行验证业务
            //context.Entry<T>(entity).GetValidationResult();
            if (CustomerDbContext.Entry<T>(entity).State == EntityState.Modified)
                CustomerDbContext.SaveChanges();
            return entity;
        }
    }
}