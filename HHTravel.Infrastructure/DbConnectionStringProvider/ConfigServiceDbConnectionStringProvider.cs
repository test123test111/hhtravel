using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using HHTravel.Base.Common.Framework.Config;

namespace HHTravel.Infrastructure
{
    public class ConfigServiceDbConnectionStringProvider : DbConnectionStringProvider
    {
        public override string GetConnctionString(string dbName)
        {
            //return ConfigHelper.GetDBConfig(dbName);
            throw new NotImplementedException();
        }
    }
}