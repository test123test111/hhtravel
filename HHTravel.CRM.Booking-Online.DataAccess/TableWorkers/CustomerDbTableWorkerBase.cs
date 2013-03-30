using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    public abstract class CustomerDbTableWorkerBase<T> : ITableWorker<T> where T : class, new()
    {
        protected CustomerDbEntities CustomerDbContext { get; set; }

        public CustomerDbTableWorkerBase()
        {
            this.CustomerDbContext = DbContextFactory.Create<CustomerDbEntities>();
        }

        public CustomerDbTableWorkerBase(CustomerDbEntities customerDbContext)
        {
            this.CustomerDbContext = customerDbContext;
        }

        public T Find(int id)
        {
            T entity = CustomerDbContext.Set<T>().Find(id);
            return entity;
        }

        public T Create()
        {
            return CustomerDbContext.Set<T>().Create();
        }

        public T Update(T entity)
        {
            //执行验证业务
            //context.Entry<T>(entity).GetValidationResult();
            if (CustomerDbContext.Entry<T>(entity).State == EntityState.Modified)
                CustomerDbContext.SaveChanges();
            return entity;
        }

        public T Insert(T entity)
        {
            CustomerDbContext.Configuration.ValidateOnSaveEnabled = false;
            CustomerDbContext.Set<T>().Add(entity);
            CustomerDbContext.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            CustomerDbContext.Set<T>().Remove(entity);
            CustomerDbContext.SaveChanges();
        }

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
    }
}
