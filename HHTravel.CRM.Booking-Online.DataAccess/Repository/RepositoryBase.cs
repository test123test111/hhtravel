using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, new()
    {
        protected DbContext DbContext { get; private set; }

        public RepositoryBase(DbContext cxt)
        {
            this.DbContext = cxt;
        }

        public abstract T Find(int id);

        public T Create()
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public T Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public abstract IEnumerable<T> All();

        public IEnumerable<T> AllIncluding(params System.Linq.Expressions.Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}
