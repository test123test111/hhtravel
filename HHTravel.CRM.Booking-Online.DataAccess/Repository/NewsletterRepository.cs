using System.Data.Entity;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.Entity;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;
using Microsoft.Practices.Unity;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public class NewsletterRepository : RepositoryBase<Subscription>, INewsletterRepository
    {
        [InjectionConstructor]
        public NewsletterRepository()
        {
            this.CustomerDbContext = DbContextFactory.Create<CustomerDbEntities>();
        }

        internal NewsletterRepository(CustomerDbEntities customerDbContext)
        {
            this.CustomerDbContext = customerDbContext;
        }

        public virtual void Subscribe(string email)
        {
            UpdateOrInsert(email, true);
        }

        public virtual void Unsubscribe(string email)
        {
            UpdateOrInsert(email, false);
        }

        protected virtual void UpdateOrInsert(string email, bool isValid)
        {
            var entity = this.CustomerDbContext.SubNews.SingleOrDefault(s => email == s.EmailAddress);
            if (entity != null)
            {
                entity.IsValid = isValid;
            }
            else
            {
                entity = new SubNews { EmailAddress = email, IsValid = isValid, };
                this.CustomerDbContext.SubNews.Add(entity);
            }

            CustomerDbContext.SaveChanges();
        }


        public override Subscription Find(int id)
        {
            throw new System.NotImplementedException();
        }

        public override System.Collections.Generic.IEnumerable<Subscription> All()
        {
            throw new System.NotImplementedException();
        }
    }
}
