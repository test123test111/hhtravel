using System;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.IRepository;

namespace HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp
{
    /// <summary>
    /// 仓储服务
    /// 提供仓储实现类的注入管理
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class RepositoryServiceImp : IRepositoryService
    {
        /// <summary>
        /// 仓储接口的类型全名
        /// </summary>
        private const string c_fromTypeFormat = "HHTravel.CRM.Booking_Online.IRepository.{0}, HHTravel.CRM.Booking-Online.IRepository";

        /// <summary>
        /// 仓储Mock类的类型全名
        /// </summary>
        private const string c_mockTypeFormat = "HHTravel.CRM.Booking_Online.Repository.Mock.{0}Mock, HHTravel.CRM.Booking-Online.Repository";

        /// <summary>
        /// 仓储实现类的的类型全名
        /// </summary>
        private const string c_toTypeFormat = "HHTravel.CRM.Booking_Online.Repository.{0}, HHTravel.CRM.Booking-Online.Repository";

        public RepositoryServiceImp()
        {
        }

        /// <summary>
        /// 实现仓储实现类的注入
        /// </summary>
        public void Register()
        {
            Register(false);
        }

        /// <summary>
        /// 实现仓储实现类的注入
        /// </summary>
        /// <param name="mock">是否使用仓储Mock类</param>
        public void Register(bool mock)
        {
            Action<string, string> registerType = (fromTypeName, toTypeName) =>
            {
                string fromTypeFullName = string.Format(c_fromTypeFormat, fromTypeName);
                Type fromType = Type.GetType(fromTypeFullName);
                string toTypeFullName = string.Format(mock ? c_mockTypeFormat : c_toTypeFormat, toTypeName);
                Type toType = Type.GetType(toTypeFullName);

                if (mock && toType == null)
                {
                    toTypeFullName = string.Format(c_toTypeFormat, toTypeName);
                    toType = Type.GetType(toTypeFullName);
                }

                if (toType == null)
                {
                    throw new ArgumentNullException(string.Format("{0}", toTypeFullName), "toType");
                }

                RepositoryFactory.RegisterType(fromType, toType);
            };

            registerType("IProductRepository", "ProductTempRepository");
            registerType("IInterestRepository", "InterestRepository");
            registerType("IDepartureMonthRepository", "DepartureMonthRepositoryLocal");
            registerType("IDestinationGroupRepository", "DestinationGroupRepository");
            registerType("INewsletterRepository", "NewsletterRepository");
            registerType("IOrderRepository", "OrderRepository");
        }
    }
}