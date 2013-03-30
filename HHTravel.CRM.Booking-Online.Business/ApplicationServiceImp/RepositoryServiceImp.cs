using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using System.ServiceModel;

namespace HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp
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
        private const string c_typeFromFormat = "HHTravel.CRM.Booking_Online.IRepository.I{0}, HHTravel.CRM.Booking-Online.IRepository";
        /// <summary>
        /// 仓储实现类的的类型全名
        /// </summary>
        private const string c_typeToFormat = "HHTravel.CRM.Booking_Online.DataAccess.Repository.{0}, HHTravel.CRM.Booking-Online.DataAccess";
        /// <summary>
        /// 仓储Mock类的类型全名
        /// </summary>
        private const string c_typeToMockFormat = "HHTravel.CRM.Booking_Online.DataAccess.Repository.Mock.{0}Mock, HHTravel.CRM.Booking-Online.DataAccess";

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
            Action<string> registerType = (typeName) =>
            {
                string typeFromName = string.Format(c_typeFromFormat, typeName);
                Type typeFrom = Type.GetType(typeFromName);
                string typeToFullName = string.Format(mock ? c_typeToMockFormat : c_typeToFormat, typeName);
                Type typeTo = Type.GetType(typeToFullName);

                if (mock && typeTo == null)
                {
                    typeToFullName = string.Format(c_typeToFormat, typeName);
                    typeTo = Type.GetType(typeToFullName);
                }

                RepositoryFactory.RegisterType(typeFrom, typeTo);
            };

            registerType("ProductRepository");
            registerType("InterestRepository");
            registerType("DepartureCityRepository");
            registerType("DepartureMonthRepository");
            registerType("DestinationGroupRepository");
            registerType("NewsletterRepository");
            registerType("SiteConfigRepository");
            registerType("OrderRepository");
            registerType("EmailRepository");
        }
    }
}
