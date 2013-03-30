using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using System.ServiceModel;

namespace HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class SiteConfigServiceImp : ISiteConfigService
    {
        private static ISiteConfigRepository s_repo;

        static SiteConfigServiceImp()
        {
            s_repo = RepositoryFactory.GetRepository<ISiteConfigRepository>();
        }

        public SiteConfigServiceImp()
        {

        }

        public SiteConfig GetSiteConfig()
        {
            var a = s_repo.GetSiteConfig();
            return a;
        }
    }
}
