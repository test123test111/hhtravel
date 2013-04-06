using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
//using HHTravel.Base.Common.Framework.Logging;
using System.Diagnostics;

namespace HHTravel.CRM.Booking_Online.Infrastructure
{
    internal class CountingCallHandler : ICallHandler
    {
        private string _countingKey;

        public CountingCallHandler(string countingKey)
        {
            _countingKey = countingKey;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn retValue = null;
            Aspect.Counting(_countingKey, () =>
            {
                retValue = getNext()(input, getNext);
            });
            return retValue;
        }

        public int Order { get; set; }
    }
}
