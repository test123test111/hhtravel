using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.IRepository
{
    public interface ISiteConfigRepository : IRepository<SiteConfig>
    {
        SiteConfig GetSiteConfig();
    }
}
