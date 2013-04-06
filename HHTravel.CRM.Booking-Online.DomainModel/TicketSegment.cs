using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    public class TicketSegment : SegmentBase
    {
        public List<Ticket> TicketList { get; set; }
    }
}