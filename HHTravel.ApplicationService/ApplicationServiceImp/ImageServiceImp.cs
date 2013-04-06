using HHTravel.DataService;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService.ApplicationServiceImp
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