using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService
{
    public interface IImageService
    {
        ImageInfo GetProductTopImage(int productId);

        ImageInfo GetTopImage(int propertyId);
    }
}