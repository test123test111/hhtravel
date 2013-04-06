using System.Collections.Generic;
using HHTravel.CRM.Booking_Online.DomainModel;

namespace DataValidator.Models
{
    public class ProductModel : Product
    {
        public ProductModel()
        {
            this.TicketList = new List<Ticket>();
            this.HotelList = new List<Hotel>();
            this.RoomClassList = new List<RoomClass>();
        }

        public string Errors { get; set; }

        public IList<Hotel> HotelList { get; set; }

        public IList<RoomClass> RoomClassList { get; set; }

        public IList<Ticket> TicketList { get; set; }
    }
}