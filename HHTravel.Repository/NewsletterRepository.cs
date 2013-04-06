using System.Linq;
using HHTravel.DataAccess;
using HHTravel.DataAccess.DbContexts;
using HHTravel.DomainModel;
using HHTravel.Entity;
using HHTravel.IRepository;
using Microsoft.Practices.Unity;

namespace HHTravel.Repository
{
    public class NewsletterRepository : RepositoryBase<Subscription>, INewsletterRepository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), InjectionConstructor]
        public NewsletterRepository()
        {
            this.CustomerDbContext = DbContextFactory.Create<CustomerDbEntities>();
        }

        internal NewsletterRepository(CustomerDbEntities customerDbContext)
        {
            this.CustomerDbContext = customerDbContext;
        }

        public override System.Collections.Generic.IEnumerable<Subscription> All()
        {
            throw new System.NotImplementedException();
        }

        public override Subscription Find(int id)
        {
            throw new System.NotImplementedException();
        }

        public Subscription Find(string email)
        {
            Subscription sub = null;
            var a = from s in CustomerDbContext.SubNews
                    where s.EmailAddress == email
                    select new Subscription
                    {
                        Email = email,
                        IsValid = (s.IsValid.HasValue && s.IsValid.Value)
                    };
            if (a.Any())
            {
                sub = a.SingleOrDefault();
            }
            return sub;
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
    }
}