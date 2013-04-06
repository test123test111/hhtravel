using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure
{
    public class MockDbConnectionStringProvider : DbConnectionStringProvider
    {
        private static Dictionary<string, string> s_dict = new Dictionary<string, string> {
            {"LOCAL", "Data Source=hhtraveldb.dev.sh.ctriptravel.com,28747;Initial Catalog={0};Persist Security Info=True;User ID=sa;Password=123456W_123456w"},
            {"DEV", "Data Source=hhtraveldb.dev.sh.ctriptravel.com,28747;Initial Catalog={0};Persist Security Info=True;User ID=sa;Password=123456W_123456w"},
            {"TEST", "Data Source=hhtraveldb.test.sh.ctriptravel.com,55666;Initial Catalog={0};Persist Security Info=True;User ID=un_liu_y;Password=123456W_123456w"},
            {"PROD TEST", "Data Source=hhtravel.db.sh.ctriptravel.com,55944;Initial Catalog={0};Persist Security Info=True;User ID=ws_hhtravel;Password=1qaz2wsx!QAZ@WSX"},
            {"PROD", "Data Source=hhtravel.db.sh.ctriptravel.com,55944;Initial Catalog={0};Persist Security Info=True;User ID=ws_hhtravel;Password=1qaz2wsx!QAZ@WSX"},
        };

        private string _connStringFormat;

        public MockDbConnectionStringProvider(string dbServer)
        {
            _connStringFormat = s_dict[dbServer];
        }

        public override string GetConnctionString(string dbName)
        {
            return string.Format(_connStringFormat, dbName);
        }
    }
}