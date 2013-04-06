using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;

namespace HHTravel.CRM.Booking_Online.DataAccess.Mock
{
    public class PicturesProviderMock : PicturesProvider
    {
        public PicturesProviderMock(ProductDbEntities productDbContext)
            : base(productDbContext)
        {
        }

        public override IQueryable<Entity.Picture> FindBy(int objectId, string objectType, string location)
        {
            var a = from p in this.All()
                    where p.ObjectId == objectId
                            && p.ObjectType == objectType
                            && p.Location == location
                            && p.URL != null
                    select p;

            return a;
        }
    }
}