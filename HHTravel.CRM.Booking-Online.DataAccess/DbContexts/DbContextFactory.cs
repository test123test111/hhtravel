using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.EntityClient;
using System.IO;
using System.Linq;
using EFProviderWrapperToolkit;
using EFTracingProvider;
using HHTravel.Base.Common.Framework.Config;
using HHTravel.Base.Common.Framework.Data;
using HHTravel.Base.Common.Framework.Logging;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess
{
    /// <summary>
    /// DbContextFactory
    /// </summary>
    public class DbContextFactory
    {
        //private static string s_logFile = "D:/log/hhtravel/ef.txt";
        private static string s_metaDataFormat =
                            string.Format("{0}{{0}}Model.csdl|{0}{{0}}Model.ssdl|{0}{{0}}Model.msl",
                                "res://HHTravel.CRM.Booking-Online.DataAccess/DbContexts.");

        private static TextWriter s_tw =
                            new ActionTextWriter(info =>
                            {
#if DEBUG
                                //using (Stream stream = File.Open(s_logFile, FileMode.OpenOrCreate))
                                //{
                                //    using (StreamWriter sw = new StreamWriter(stream))
                                //    {
                                //        sw.WriteLine(info);
                                //    }
                                //}
#else
                                // 默认记录到数据库中
                                // SysLog.WriteTrace(info);
#endif
                            });

        /// <summary>
        /// 初始化各具体DbContext对象的构造参数
        /// </summary>
        private static Dictionary<Type, DbContextConstructionArgumentBag<DbEntitiesBase>> s_dict =
            new Dictionary<Type, DbContextConstructionArgumentBag<DbEntitiesBase>>{
                { typeof(ProductDbEntities), 
                     new DbContextConstructionArgumentBag<DbEntitiesBase>(SqlHelper.ProductDB, 
                    "ProductDb", 
                    (existingConnection, contextOwnsConnection)=> new ProductDbEntities(existingConnection, contextOwnsConnection),
                    (connectionString) => new ProductDbEntities(connectionString))
                },
                { typeof(CustomerDbEntities),  
                    new DbContextConstructionArgumentBag<DbEntitiesBase>(SqlHelper.CustomerDB, 
                    "CustomerDb", 
                    (existingConnection, contextOwnsConnection)=> new CustomerDbEntities(existingConnection, contextOwnsConnection), 
                    (connectionString) => new CustomerDbEntities(connectionString))
                },
                { typeof(OrderDbEntities),  
                    new DbContextConstructionArgumentBag<DbEntitiesBase>(SqlHelper.OrderDB, 
                    "OrderDb", 
                    (existingConnection, contextOwnsConnection)=> new OrderDbEntities(existingConnection, contextOwnsConnection), 
                    (connectionString) => new OrderDbEntities(connectionString))
                },
                { typeof(GovDbEntities),  
                    new DbContextConstructionArgumentBag<DbEntitiesBase>(SqlHelper.GovDB, 
                    "GovDb", 
                    (existingConnection, contextOwnsConnection)=> new GovDbEntities(existingConnection, contextOwnsConnection), 
                    (connectionString) => new GovDbEntities(connectionString))
                },
        };

        private static bool s_enableTracing = true;
        /// <summary>
        /// 是否附加Trace机制
        /// </summary>
        public static bool EnableTracing
        {
            get { return s_enableTracing; }
            set { s_enableTracing = value; }
        }
        private static bool s_enableCaching = true;
        /// <summary>
        /// 是否附加缓存机制
        /// </summary>
        public static bool EnableCaching
        {
            get { return s_enableCaching; }
            set { s_enableCaching = value; }
        }

        public static TextWriter Log
        {
            get
            {
                return s_tw;
            }
            set
            {
                if (value != null)
                {
                    s_tw = value;
                }
            }
        }

        /// <summary>
        /// 创建DbContext对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
            where T : DbEntitiesBase
        {
            T context = null;
            var bag = s_dict.SingleOrDefault(kvp => typeof(T) == kvp.Key).Value;

            if (bag == null)
            {
                throw new NotSupportedException(string.Format("请检查s_dict是否加入了对类型{0} 的支持", typeof(T).FullName));
            }

            // 从数据库配置表中读取数据库连接串的取值
            string connStr = ConfigHelper.GetDBConfig(bag.ConnectionStringKey);

            // 转换为EF的连接串格式
            string entityConnStr = GetEntityConnectionString(connStr, bag.ModelPrefix);

            if (EnableTracing || EnableCaching)
            {
                DbConnection existingConnection;
                List<string> providers = new List<string>();
                if (EnableTracing)
                    providers.Add("EFTracingProvider");
                if (EnableCaching)
                    providers.Add("EFCachingProvider");

                try
                {
                    existingConnection = EntityConnectionWrapperUtils.CreateEntityConnectionWithWrappers(entityConnStr,
                               providers.ToArray());
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("请检查*.edmx文件的位置。若文件重命名/命名空间改变，需要修改s_metaDataFormat的值。" + entityConnStr, ex);
                }

                context = bag.Constructor1(existingConnection, true) as T;
                ((IObjectContextAdapter)context).ObjectContext.EnableTracing();
                context.Log = s_tw;
            }
            else
            {
                context = bag.Constructor2(entityConnStr) as T;
            }

            return context;
        }
        private static EntityConnectionStringBuilder s_efConnSB =
                            new EntityConnectionStringBuilder { Provider = "System.Data.SqlClient" };
        /// <summary>
        /// 生成EF需要的连接串
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public static string GetEntityConnectionString(string connectionString, string modelName)
        {
            // 支持多数据结果集MARS
            connectionString = string.Format("{0};MultipleActiveResultSets=true;App=EntityFramework", connectionString);
            s_efConnSB.ProviderConnectionString = connectionString;
            s_efConnSB.Metadata = string.Format(s_metaDataFormat, modelName);

            return s_efConnSB.ToString();
        }

        /// <summary>
        /// 具体DbContext对象的构造参数。
        /// 辅助Factory的生产过程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class DbContextConstructionArgumentBag<T>
            where T : DbEntitiesBase
        {
            /// <summary>
            /// 待创建的DbContext对象的Type
            /// </summary>
            public Type DbEntitiesBaseType { get; set; }
            /// <summary>
            /// 数据库连接串在配置表中的Key
            /// </summary>
            public string ConnectionStringKey { get; set; }
            /// <summary>
            /// EF实体模型的命名前缀
            /// </summary>
            public string ModelPrefix { get; set; }
            /// <summary>
            /// 用于注入具体DbContext类的构造函数。
            /// 构造函数的原型为(existingConnection, contextOwnsConnection)
            /// </summary>
            public Func<DbConnection, bool, T> Constructor1 { get; set; }
            /// <summary>
            /// 用于注入具体DbContext类的构造函数。
            /// 构造函数的原型为(connectionString)
            /// </summary>
            public Func<string, T> Constructor2 { get; set; }

            public DbContextConstructionArgumentBag(string connectionStringKey, string modelPrefix, Func<DbConnection, bool, T> constructor1, Func<string, T> constructor2)
            {
                this.ConnectionStringKey = connectionStringKey;
                this.ModelPrefix = modelPrefix;
                this.Constructor1 = constructor1;
                this.Constructor2 = constructor2;
            }
        }
    }
}
