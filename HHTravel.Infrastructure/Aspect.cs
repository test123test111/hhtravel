using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
//using HHTravel.Base.Common.Framework.Logging;

namespace HHTravel.CRM.Booking_Online.Infrastructure
{
    public static class Aspect
    {
        public static void Counting(string countingKey, Action block)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            block();


            sw.Stop();

            int elapsedMilliseconds = sw.ElapsedMilliseconds > int.MaxValue ? int.MaxValue : (int)sw.ElapsedMilliseconds;
            //Counter.AddCounter(countingKey, elapsedMilliseconds);
        }
    }
}
