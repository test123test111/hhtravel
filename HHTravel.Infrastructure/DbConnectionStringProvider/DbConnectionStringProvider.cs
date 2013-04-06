using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure
{
    public abstract class DbConnectionStringProvider : ProviderBase
    {
        public abstract string GetConnctionString(string dbName);

        static DbConnectionStringProvider()
        {
            Instance = new ConfigServiceDbConnectionStringProvider();
        }

        public static DbConnectionStringProvider Instance { get; set; }
    }
}