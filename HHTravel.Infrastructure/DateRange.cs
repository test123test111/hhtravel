using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.Infrastructure
{
    public class DateRange : IEnumerable<DateTime>
    {
        private DateTime _beginDate;

        private DateTime _endDate;

        public DateRange(DateTime beginDate, DateTime endDate)
        {
            _beginDate = beginDate;
            _endDate = endDate;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            for (DateTime date = this._beginDate; date <= this._endDate; date = date.AddDays(1))
            {
                yield return date;
            }
        }
    }
}