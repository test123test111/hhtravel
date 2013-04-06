using System.Linq;
using HHTravel.DataAccess.DbContexts;
using HHTravel.DataAccess.Providers;

namespace HHTravel.DataAccess.Mock
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