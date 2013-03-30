using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class LefuMenuModel
    {
        public IEnumerable<DestinationGroup> DestinationGroups { get; set; }
        public IEnumerable<Interest> Interests { get; set; }
        public IEnumerable<DepartureMonth> DepartureMonths { get; set; }
    }
}