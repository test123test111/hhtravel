using System;
using System.Collections.Generic;
using System.Linq;

namespace HHTravel.CRM.Booking_Online.DataAccess
{
    public class PagerHelper
    {
        private PagerHelper()
        {
        }

        public static IQueryable<T> LazyTakePage<T>(IQueryable<T> query, int pageSize, int? pageIndex, out int pageCount) where T : class
        {
            int count;
            count = query.Count();
            pageCount = (int)Math.Ceiling((decimal)count / pageSize);

            int i = 0;
            if (pageIndex.HasValue)
            {
                i = pageIndex.Value;

                // fix range
                if (i < 0) i = 0;
            }

            // fix range
            if (pageIndex > pageCount)
                pageIndex = pageCount;

            int skip = i * pageSize;
            int take = pageSize;

            return query.Skip(skip).Take(take);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakePage<T>(IEnumerable<T> query, int pageSize, int? pageIndex, out int pageCount) where T : class
        {
            int count;
            count = query.Count();

            pageCount = (int)Math.Ceiling((decimal)count / pageSize);

            int i = 0;
            if (pageIndex.HasValue)
            {
                i = pageIndex.Value;

                // fix range
                if (i < 0) i = 0;
            }

            // fix range
            if (pageIndex > pageCount)
                pageIndex = pageCount;

            int skip = i * pageSize;
            int take = pageSize;

            return query.Skip(skip).Take(take);
        }
    }
}