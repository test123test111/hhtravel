using HHTravel.CRM.Booking_Online.DataService;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp
{
    public class ImageServiceImp : IImageService
    {
        public ImageInfo GetProductTopImage(int productId)
        {
            return ImageService.GetProductTopImage(productId);
        }

        public ImageInfo GetTopImage(int propertyId)
        {
            return ImageService.GetTopImage(propertyId);
        }
    }
}