using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class, new()
    {
        protected ProductDbEntities ProductDbContext { get; set; }
        protected CustomerDbEntities CustomerDbContext { get; set; }
        protected OrderDbEntities OrderDbContext { get; set; }
        protected GovDbEntities GovDbContext { get; set; }

        public abstract T Find(int id);

        public abstract IEnumerable<T> All();
    }
}
