using System;
using System.Configuration;
using System.Globalization;

namespace HHTravel.Infrastructure
{
    public class GlobalConfig
    {
        #region 表现层

        /// <summary>
        /// 是否关闭百度统计
        /// </summary>
        public static bool CloseBaiDuStatistics = GetFromAppSettings<bool>("CloseBaiDuStatistics", false);

        /// <summary>
        /// 是否检测浏览器语言 默认：不检测
        /// </summary>
        public static bool DetectBrowserLanguage = GetFromAppSettings<bool>("DetectBrowserLanguage", false);

        /// <summary>
        /// 使用测试版本的营销HTML
        /// </summary>
        public static bool UseTestMarketingHtml = GetFromAppSettings<bool>("UseTestMarketingHtml", false);

        #endregion 表现层

        /// <summary>
        /// 是否包含TravelType3的产品 默认：不包含
        /// </summary>
        public static bool ContainsProductsIsSingleProduct = GetFromAppSettings<bool>("ContainsProductsIsSingleProduct", false);

        /// <summary>
        /// 是否按生产环境处理 默认：是
        /// </summary>
        public static bool IsProductEnvironment = GetFromAppSettings<bool>("IsProductEnvironment", true);

        #region EF

        /// <summary>
        /// EFCaching缓存过期时间 默认：10分钟
        /// </summary>
        public static int EFCachingExpiration = GetFromAppSettings<int>("EFCachingExpiration", 10);

        /// <summary>
        /// 是否启用EFCaching 默认：启用
        /// </summary>
        public static bool EnableEFCaching = GetFromAppSettings<bool>("EnableEFCaching", true);

        /// <summary>
        /// 是否启用EFTracing 默认：启用
        /// </summary>
        public static bool EnableEFTracing = GetFromAppSettings<bool>("EnableEFTracing", true);

        #endregion EF

        /// <summary>
        /// 是否使用Mock的Repository实现 默认：不使用
        /// </summary>
        public static bool MockRepository = GetFromAppSettings<bool>("MockRepository", false);

        public static T? ConvertToNullable<T>(object obj) where T : struct
        {
            T? value = default(T?);

            var nullableValue = obj as T?;
            if (nullableValue.HasValue)
            {
                return nullableValue;
            }

            if (obj == null || Convert.IsDBNull(obj))
            {
                return null;
            }

            try
            {
                value = (T)Convert.ChangeType(obj, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
            }

            return value;
        }

        public static T GetFromAppSettings<T>(string key, T defaultValue) where T : struct
        {
            T value;
            var v = ConfigurationManager.AppSettings[key];

            try
            {
                // boxing
                value = (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);
            }
            catch
            {
                value = defaultValue;
            }

            return value;
        }
    }
}