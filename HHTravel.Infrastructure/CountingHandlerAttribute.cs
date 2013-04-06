using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace HHTravel.Infrastructure
{
    public class CountingHandlerAttribute : HandlerAttribute
    {
        public CountingHandlerAttribute(string countingKey)
        {
            this.CountingKey = countingKey;
        }

        public string CountingKey { get; set; }

        public override ICallHandler CreateHandler(Microsoft.Practices.Unity.IUnityContainer container)
        {
            CountingCallHandler handler = new CountingCallHandler(this.CountingKey);
            return handler;
        }
    }
}
