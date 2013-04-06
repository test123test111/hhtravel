using System;
using System.Linq;
using HHTravel.DataAccess.DbContexts;
using HHTravel.Entity;

namespace HHTravel.DataAccess.Providers
{
    public class PicturesProvider : ProductDbProviderBase<Picture>
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public PicturesProvider()
            : base(DbContextFactory.Create<ProductDbEntities>())
        {
        }

        public PicturesProvider(ProductDbEntities productDbContext)
            : base(productDbContext)
        {
        }

        public override Picture Find(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<Picture> FindBy(string objectType)
        {
            var a = from p in this.All()
                    where p.ObjectType == objectType
                            && p.URL != null
                    select p;

            return a;
        }

        public virtual IQueryable<Picture> FindBy(int objectId, string objectType)
        {
            var a = from p in this.All()
                    where p.ObjectId == objectId
                            && p.ObjectType == objectType
                            && p.URL != null
                    select p;

            return a;
        }

        public virtual IQueryable<Picture> FindBy(string objectType, string location)
        {
            var a = from p in this.All()
                    where p.ObjectType == objectType
                            && p.URL != null
                    select p;
            if (location != null)
            {
                a = a.Where(p => p.Location == location);
            }
            return a;
        }

        public virtual IQueryable<Picture> FindBy(int objectId, string objectType, string location)
        {
            var a = from p in this.All()
                    where p.ObjectId == objectId
                            && p.ObjectType == objectType
                            && p.URL != null
                    select p;
            if (location != null)
            {
                a = a.Where(p => p.Location == location);
            }
            return a;
        }
    }
}