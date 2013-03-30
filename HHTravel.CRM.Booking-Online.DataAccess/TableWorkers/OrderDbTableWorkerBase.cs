using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    public abstract class OrderDbTableWorkerBase<T> : ITableWorker<T> where T : class, new()
    {
        protected OrderDbEntities OrderDbContext { get; set; }

        public OrderDbTableWorkerBase()
        {
            this.OrderDbContext = DbContextFactory.Create<OrderDbEntities>();
        }

        public OrderDbTableWorkerBase(OrderDbEntities orderDbContext)
        {
            this.OrderDbContext = orderDbContext;
        }

        public T Find(int id) 
        {
            var entity = OrderDbContext.Set<T>().Find(id);
            return entity;
        }

        public T Create()
        {
            return OrderDbContext.Set<T>().Create();
        }

        public T Update(T entity)
        {
            //执行验证业务
            //context.Entry<T>(entity).GetValidationResult();
            if (OrderDbContext.Entry<T>(entity).State == EntityState.Modified)
                OrderDbContext.SaveChanges();
            return entity;
        }

        public T Insert(T entity)
        {
            OrderDbContext.Set<T>().Add(entity);
            OrderDbContext.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            OrderDbContext.Set<T>().Remove(entity);
            OrderDbContext.SaveChanges();
        }

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
    }
}
