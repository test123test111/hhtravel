using System;

namespace HHTravel.Infrastructure
{
    public static class DateTimeExtensions
    {
        // http://stackoverflow.com/questions/1525990/difference-in-months
        public static int MonthDifference(this DateTime lValue, DateTime rValue)
        {
            return Math.Abs((lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year));
        }
    }
}