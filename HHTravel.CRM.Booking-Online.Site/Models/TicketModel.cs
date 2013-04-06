using System.Collections.Generic;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class TicketModel
    {
        public string AdditionalPurchasesNote { get; set; }

        /// <summary>
        /// 机票张数
        /// </summary>
        public short Count { get; set; }

        /// <summary>
        /// 舱等
        /// </summary>
        public string FlightClassName { get; set; }

        /// <summary>
        /// 航班
        /// </summary>
        public List<FlightModel> FlightModelList { get; set; }

        /// <summary>
        /// 价格
        /// （非TravelType3的机票目前未设价格 2012-11-13）
        /// </summary>
        public decimal? Price { get; set; }

        public decimal PriceDelta { get; set; }

        public int TicketId { get; set; }
    }
}