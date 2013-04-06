using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class SearchBoxModel
    {
        public string DaysSection { get; set; }

        public IEnumerable<SelectListItem> DaysSections { get; set; }

        public DateTime DepartureBeginDate { get; set; }

        public IEnumerable<DepartureCity> DepartureCitys { get; set; }

        public DateTime DepartureEndDate { get; set; }

        public IEnumerable<DestinationGroup> DestinationGroups { get; set; }

        public IEnumerable<Interest> Interests { get; set; }

        public IEnumerable<TravelType> TravelTypes { get; set; }
    }
}