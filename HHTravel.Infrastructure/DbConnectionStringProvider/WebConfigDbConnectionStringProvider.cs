using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.Infrastructure
{
    public class WebConfigDbConnectionStringProvider : DbConnectionStringProvider
    {
        public override string GetConnctionString(string dbName)
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
        }
    }
}