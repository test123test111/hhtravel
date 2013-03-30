using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class SelectedTicketModel : Ticket
    {
        /// <summary>
        /// 机票张数
        /// </summary>
        public short Count { get; set; }
    }
}