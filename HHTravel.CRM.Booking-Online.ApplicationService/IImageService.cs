using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.ApplicationService
{
    public interface IImageService
    {
        ImageInfo GetProductTopImage(int productId);

        ImageInfo GetTopImage(int propertyId);
    }
}