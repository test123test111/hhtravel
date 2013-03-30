using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    internal interface ITableWorker<T> where T : class, new()
    {
        T Create();
        T Update(T entity);
        void Delete(T entity);
        T Insert(T entity);

        T Find(int id);
        //T Find(params object[] keyValues);
        IQueryable<T> All();
        // The Include path expression must refer to a navigation property defined on the type. 
        // Use dotted paths for reference navigation properties and the Select operator for collection navigation properties.
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
    }
}
