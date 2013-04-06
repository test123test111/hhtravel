using System;
using System.Collections.Generic;

namespace HHTravel.Site.Models
{
    public class DestinationModel
    {
        public DateTime CheckinDate { get; set; }

        public DateTime CheckoutDate { get; set; }

        public List<HotelModel> HotelList { get; set; }

        /// <summary>
        /// 几晚
        /// </summary>
        public int Nights { get; set; }
    }
}