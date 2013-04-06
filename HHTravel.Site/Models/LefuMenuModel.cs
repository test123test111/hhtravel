using System.Collections.Generic;
using HHTravel.DomainModel;

namespace HHTravel.Site.Models
{
    public class LefuMenuModel
    {
        public IEnumerable<DepartureMonth> DepartureMonths { get; set; }

        public IEnumerable<DestinationGroup> DestinationGroups { get; set; }

        public IEnumerable<Interest> Interests { get; set; }
    }
}