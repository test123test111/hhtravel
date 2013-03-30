using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.DataAccess
{
    public class Pager
    {
        internal static int PageSize = 5;

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        internal static IEnumerable<T> TakePage<T>(IEnumerable<T> list, int? pageIndex, out int pageCount)
        {
            int count = list.Count();
            pageCount = (int)Math.Ceiling((decimal)count / PageSize);

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

            int skip = i * PageSize;
            int take = PageSize;

            var b = list.Skip(skip).Take(take);
            return b;
        }
    }
}
