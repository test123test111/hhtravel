using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace HHTravel.CRM.Booking_Online.DataAccess.Mock
{
    /// <summary>
    /// Mock辅助工具类
    /// </summary>
    public class MockHelper
    {
        /// <summary>
        /// 从指定集合中随机获取一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalList"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(IEnumerable<T> originalList)
        {
            return GetRandomItem(null, originalList, false);
        }
        /// <summary>
        /// 从指定集合中随机获取一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seed"></param>
        /// <param name="originalList"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(int? seed, IEnumerable<T> originalList)
        {
            return GetRandomItem(seed, originalList, false);
        }
        /// <summary>
        /// 从指定集合中随机获取一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalList"></param>
        /// <param name="tryReturnsTypeDefaultValue"></param>
        /// <returns></returns>
        public static T GetRandomItem<T>(IEnumerable<T> originalList, bool tryReturnsTypeDefaultValue)
        {
            return GetRandomItem(null, originalList, tryReturnsTypeDefaultValue);
        }

        /// <summary>
        /// 从指定集合中随机获取一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalList"></param>
        /// <param name="tryReturnsTypeDefaultValue">是否尝试用T的默认值作为返回对象</param>
        /// <returns></returns>
        public static T GetRandomItem<T>(int? seed, IEnumerable<T> originalList, bool tryReturnsTypeDefaultValue)
        {
            var count = originalList.Count();
            if (count == 0)
                return default(T);

            Random r = seed.HasValue ? new Random(seed.Value) : new Random();
            var index = r.Next(0, count);

            bool isReturnsTypeDefaultValue = (tryReturnsTypeDefaultValue && r.Next(0, 2) == 1);
            if (isReturnsTypeDefaultValue)
                return default(T);

            var result = originalList.Skip(index).First();
            return result;
        }
        /// <summary>
        /// 从指定集合中随机获取一个子集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="originalList"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(IEnumerable<T> originalList)
        {
            var result = GetRandomList(null, originalList);
            return result;
        }
        /// <summary>
        /// 从指定集合中随机获取一个子集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="seed"></param>
        /// <param name="originalList"></param>
        /// <returns></returns>
        public static List<T> GetRandomList<T>(int? seed, IEnumerable<T> originalList)
        {
            var count = originalList.Count();
            if (count == 0)
                return new List<T>();

            Random r = seed.HasValue ? new Random(seed.Value) : new Random();
            var index = r.Next(0, count - 1);
            var length = r.Next(1, count - index - 1);
            var result = originalList.Skip(index).Take(length).ToList();
            return result;
        }
        /// <summary>
        /// 获取一个在指定时间范围内的一个DateTime对象
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static DateTime GetRandomDate(DateTime beginDate, DateTime endDate)
        {
            var a = GetRandomItem<DateTime>(GetRandomDateList(beginDate, endDate));
            return a;
        }
        /// <summary>
        /// 获取一个在指定时间范围内的DateTime对象的集合
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static List<DateTime> GetRandomDateList(DateTime beginDate, DateTime endDate)
        {
            List<DateTime> list = new List<DateTime>();
            var ts = endDate - beginDate;
            int days = ts.Days;

            Random r = new Random();
            int count = r.Next(3, 8);   // HARDCODE: 生成3~8个日期

            for (int i = 0; i < count; i++)
            {
                int d = r.Next(1, days);
                DateTime date = beginDate.AddDays(d);
                list.Add(date);
            }

            return list;
        }

        public static List<T> GetList<T>(Func<List<T>> build)
        {
            List<T> list = new List<T>();
            FileStream stream;
            XmlSerializer seri = new XmlSerializer(typeof(List<T>));
            string basePath = "D:/log/hhtravel/";

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            string c_cacheFilePath = Path.Combine(basePath, typeof(T).Name) + ".txt";

            if (File.Exists(c_cacheFilePath))
            {
                using (StreamReader sr = new StreamReader(c_cacheFilePath))
                {
                    list = (List<T>)seri.Deserialize(sr);
                }
            }
            else
            {
                stream = File.Create(c_cacheFilePath);
                list = build();

                using (StreamWriter sw = new StreamWriter(stream))
                {
                    seri.Serialize(sw, list);
                }
            }
            return list;
        }
    }
}
