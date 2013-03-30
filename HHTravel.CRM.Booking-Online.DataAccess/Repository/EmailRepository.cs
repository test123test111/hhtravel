using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;
using Microsoft.Practices.Unity;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public class EmailRepository : RepositoryBase<Email>, IEmailRepository
    {
        [InjectionConstructor]
        public EmailRepository()
        {
            this.GovDbContext = DbContextFactory.Create<GovDbEntities>();
        }

        internal EmailRepository(GovDbEntities govDbContext)
        {
            this.GovDbContext = govDbContext;
        }

        /// <summary>
        /// 插入待发邮件表
        /// </summary>
        /// <param name="email"></param>
        public void Insert(Email email)
        {
            string emailType = email.EmailType.ToString();

            Entity.Email eEmail = new Entity.Email{
                Sender = email.Sender,
                Recipient = email.Recipients,
                Cc = email.Cc,
                Subject=email.Subject,
                Body = email.Body,
                EmailType = emailType,
                OrderID = email.OrderId,
                CustomerID = email.CustomerId,
            };

            GovDbContext.Email.Add(eEmail);
            GovDbContext.SaveChanges();
        }

        public override Email Find(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<Email> All()
        {
            throw new NotImplementedException();
        }
    }
}
