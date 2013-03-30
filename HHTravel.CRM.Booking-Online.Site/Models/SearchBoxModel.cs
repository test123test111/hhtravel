using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Model;
using System.Web.WebPages.Html;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class SearchBoxModel
    {
        public IEnumerable<DepartureCity> DeparturePlaces { get; set; }
        public IEnumerable<DestinationGroup> DestinationGroups { get; set; }
        public DateTime DepartureBeginDate { get; set; }
        public DateTime DepartureEndDate { get; set; }
        public IEnumerable<TravelType> TravelTypes { get; set; }
        public IEnumerable<Interest> Interests { get; set; }
    }
}