using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HHTravel.Site.Models
{
    public class CalendarDayModel
    {
        /// <summary>
        /// 是否属于别的月份
        /// </summary>
        public bool BelongsOtherMonth { get; set; }

        public string DateString { get; set; }

        /// <summary>
        /// 日期中的“天”部分
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// 是否额满
        /// </summary>
        public bool IsSetOff { get; set; }

        /// <summary>
        /// 最少成团人数（TravelType1/4）
        /// </summary>
        public int MinGroupNum { get; set; }

        /// <summary>
        /// 起价
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Price { get; set; }

        internal DateTime Date { get; set; }
    }

    public class CalendarModel
    {
        public CalendarModel(DateTime beginDate, DateTime endDate, int year, int month)
        {
            beginDate = (beginDate.Date >= DateTime.Now.Date) ? beginDate.Date : DateTime.Now.Date;

            this.BeginYear = beginDate.Year;
            this.BeginMonth = beginDate.Month;
            this.EndYear = endDate.Year;
            this.EndMonth = endDate.Month;

            if (year < DateTime.Now.Year ||
                (year == DateTime.Now.Year && month < DateTime.Now.Month))
            {
                return;
            }

            if (year == DateTime.Now.Year && month == DateTime.Now.Month
                && beginDate > DateTime.Now.Date)
            {
                year = beginDate.Year;
                month = beginDate.Month;
            }

            if (year > endDate.Year ||
                (year == endDate.Year && month > endDate.Month))
            {
                return;
            }

            this.MonthModel = new CalendarMonthModel(year, month);
        }

        public int BeginMonth { get; private set; }

        public int BeginYear { get; private set; }

        public int EndMonth { get; private set; }

        public int EndYear { get; private set; }

        public CalendarMonthModel MonthModel { get; set; }
    }

    public class CalendarMonthModel
    {
        // 日历中每个月显示多少个格子（天）
        private static readonly int s_daysInMonthView = 42;

        public CalendarMonthModel(int year, int month)
        {
            this.Year = year;
            this.Month = month;

            var beginDate = GetBeginDateInCalendar(year, month);
            var dates = GetDates(beginDate, s_daysInMonthView);

            // 装填DayModel
            this.Days = dates.Select(d => new CalendarDayModel
            {
                Day = d.Day,
                Date = d.Date,
                DateString = d.Date.ToString("yyyy-MM-dd"),
                BelongsOtherMonth = (d.Month != month),
            }).ToList();
        }

        public List<CalendarDayModel> Days { get; set; }

        /// <summary>
        /// 是否有可出发日期
        /// </summary>
        public bool HasDepartureDate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public static DateTime GetBeginDateInCalendar(int year, int month)
        {
            DateTime beginDate;
            var firstDay = new DateTime((int)year, (int)month, 1);
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
                beginDate = new DateTime((int)year, (int)month, day);
            }

            return beginDate;
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
    }
}