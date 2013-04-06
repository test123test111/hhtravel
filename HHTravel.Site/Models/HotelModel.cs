using System.Collections.Generic;
using HHTravel.DomainModel;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.Site.Models
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