using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HHTravel.CRM.Booking_Online.DataAccess.Providers
{
    internal interface IDataProvider<T> : IDisposable where T : class, new()
    {
        //T Find(params object[] keyValues);
        IQueryable<T> All();

        // The Include path expression must refer to a navigation property defined on the type.
        // Use dotted paths for reference navigation properties and the Select operator for collection navigation properties.
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);

        T Create();

        void Delete(T entity);

        T Find(params object[] keyValues);

        T Insert(T entity);

        T Update(T entity);
    }
}