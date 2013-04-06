using System.Collections.Generic;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class SitemapModel
    {
        public IEnumerable<DepartureCity> DepartureCitys { get; set; }

        public IEnumerable<DepartureMonth> DepartureMonths { get; set; }

        public IEnumerable<DestinationGroup> DestinationGroups { get; set; }

        public IEnumerable<Interest> Interests { get; set; }
    }
}