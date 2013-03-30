using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using Microsoft.Practices.Unity;
using System.Data.Entity;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.DataAccess.HardCode;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.DataAccess.TableWorkers
{
    internal class PictureTableWorker : ProductDbTableWorkerBase<Picture>
    {
        public PictureTableWorker()
            : base(DbContextFactory.Create<ProductDbEntities>())
        {

        }

        public PictureTableWorker(ProductDbEntities productDbContext)
            : base(productDbContext)
        {

        }

        public override Picture Find(int id)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<Picture> FindBy(int objectId, string objectType)
        {
            var a = from p in ProductDbContext.Picture
                    where p.ObjectId == objectId
                            && p.ObjectType == objectType
                            && p.URL != null
                    select p;

            return a;
        }

        public virtual IQueryable<Picture> FindBy(string objectType, string location)
        {
            var a = from p in ProductDbContext.Picture
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
            var a = from p in ProductDbContext.Picture
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
