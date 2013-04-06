using System.Collections.Generic;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class HotelModel : ITopImageModel
    {
        public HotelModel()
        {
            RoomClassModelList = new List<RoomClassModel>();
        }

        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public string Description { get; set; }

        public List<RoomClassModel> RoomClassModelList { get; set; }

        public ImageInfo TopImage { get; set; }

        public bool Checked { get; set; }
    }
}