using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class TicketSegmentModel
    {
        public TicketModel MinPriceTicketModule { get; set; }

        /// <summary>
        /// 机票
        /// </summary>
        public List<TicketModel> TicketModelList { get; set; }
    }
}