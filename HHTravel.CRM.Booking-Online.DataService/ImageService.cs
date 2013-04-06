using System.Linq;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.DataService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ImageService
    {
        public static ImageInfo GetProductTopImage(int productId)
        {
            var a = (from pic in GetAllTopImage(PictureObjectType.产品)
                     where pic.ObjectId == productId
                     select pic).FirstOrDefault();
            var imageInfo = GetImageInfo(a);
            return imageInfo;
        }

        // todo: 性能优化
        public static ImageInfo GetTopImage(int propertyId)
        {
            var a = (from pic in GetAllTopImage(PictureObjectType.属性)
                     where pic.ObjectId == propertyId
                     select pic).FirstOrDefault();
            var imageInfo = GetImageInfo(a);
            return imageInfo;
        }

        private static IQueryable<Entity.Picture> GetAllTopImage(string objectType)
        {
            PicturesProvider picsProvider = new PicturesProvider();
            var a = (from pic in picsProvider.FindBy(objectType, PictureLocation.营销图)
                     select pic);
            return a;
        }

        private static ImageInfo GetImageInfo(HHTravel.CRM.Booking_Online.Entity.Picture pic)
        {
            if (pic == null || string.IsNullOrEmpty(pic.URL)) return null;
            return new ImageInfo { Title = pic.Title, Url = pic.URL };
        }
    }
}