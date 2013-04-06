using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class MonthModel
    {
        // 日历中每个月显示多少个格子（天）
        private static readonly int s_daysInMonthView = 42;

        public int Year { get; set; }
        public int Month { get; set; }
        public List<DayModel> Days { get; set; }

        public MonthModel(int year, int month)
        {
            this.Year = year;
            this.Month = month;

            var beginDate = GetBeginDateInCalendar(year, month);
            var dates = GetDates(beginDate, s_daysInMonthView);
            // 装填DayModel
            this.Days = dates.Select(d => new DayModel
            {
                Day = d.Day,
                Date = d.Date,
                DateString = d.Date.ToString("yyyyMMdd"),
                BelongsOtherMonth = (d.Month != month),
            }).ToList();
        }

        private List<DateTime> GetDates(DateTime beginDate, int length)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime endDate = beginDate.AddDays(length);

            for (int i = 0; i < length; i++)
            {
                var date = beginDate.AddDays(i);
                dates.Add(date);
            }

            return dates;
        }

        public static DateTime GetBeginDateInCalendar(int year, int month)
        {
            DateTime beginDate;
            var firstDay = new DateTime(year, month, 1);
            int lastMonthDays = (firstDay - firstDay.AddMonths(-1)).Days;

            if (firstDay.DayOfWeek == DayOfWeek.Sunday)
            {
                beginDate = firstDay;
            }
            else
            {
                if (month == 1)
                {
                    year = year - 1;
                    month = 12;
                }
                else
                {
                    month = month - 1;
                }

                var day = lastMonthDays - (int)firstDay.DayOfWeek + 1;
                beginDate = new DateTime(year, month, day);
            }

            return beginDate;
        }
    }

    public class DayModel
    {
        internal DateTime Date { get; set; }
        /// <summary>
        /// 是否属于别的月份
        /// </summary>
        public bool BelongsOtherMonth { get; set; }
        /// <summary>
        /// 日期中的“天”部分
        /// </summary>
        public int Day { get; set; }

        public string DateString { get; set; }
        /// <summary>
        /// 最少成行人数（自由行）
        /// </summary>
        public int MinPersonNum { get; set; }
        /// <summary>
        /// 起价
        /// </summary>
        public int Price { get; set; }
    }
}