using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using System.Data.Entity;

namespace HHTravel.CRM.Booking_Online.DataAccess.Mock
{
    internal class PictureTableWorkerMock : PictureTableWorker
    {
        public PictureTableWorkerMock(ProductDbEntities productDbContext)
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
