using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public interface ITopImageModel
    {
        ImageInfo TopImage { get; set; }
    }
}