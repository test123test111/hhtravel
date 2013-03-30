using System;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Business.IApplicationService
{
    [ServiceContract]
    public interface ISiteConfigService
    {
        [OperationContract]
        SiteConfig GetSiteConfig();
    }
}
