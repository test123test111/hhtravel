using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HHTravel.Infrastructure.Crosscutting
{
    /// <summary>
    /// 出发地
    /// </summary>
    [DataContract]
    public struct DepartureCity
    {
        public static readonly string 台北 = "TPE";
        public static readonly string 香港 = "HKG";

        private static Dictionary<string, string> s_dict = new Dictionary<string, string> {
            { "SHA", "上海" },
            { "BJS", "北京" },
            //{ "CAN", "广州" },
            //{ "CTU", "成都" },
            { "TPE", "台北" },
            //{ "HKG", "香港" },
        };

        [DataMember]
        public string CityCode { get; private set; }

        [DataMember]
        public string CityName { get; private set; }

        public static IEnumerable<DepartureCity> All()
        {
            var a = from i in s_dict
                    select new DepartureCity { CityCode = i.Key.ToString(), CityName = i.Value };
            return a;
        }

        /// <summary>
        /// 从城市代码转化为对象
        /// 忽略大小写
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DepartureCity? Parse(string code)
        {
            DepartureCity? departureCity = null;
            var c = GetItem(code);
            if (c != null)
            {
                var kvp = c.Value;
                departureCity = new DepartureCity
                {
                    CityCode = kvp.Key,
                    CityName = kvp.Value,
                };
            }
            return departureCity;
        }

        private static KeyValuePair<string, string>? GetItem(string code)
        {
            KeyValuePair<string, string>? item = null;
            var a = (from i in s_dict
                     where string.Equals(i.Key, code, StringComparison.OrdinalIgnoreCase)
                     select i);
            if (a.Any())
            {
                item = a.Single();
            }
            return item;
        }
    }
}