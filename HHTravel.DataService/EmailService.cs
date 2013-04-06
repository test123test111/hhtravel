using System.ServiceModel;
using HHTravel.DataAccess;
using HHTravel.DataAccess.DbContexts;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.DataService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EmailService
    {
        public static void Add(Email email)
        {
            string emailType = email.EmailType.ToString();
            Entity.Email eEmail = new Entity.Email
            {
                Sender = email.Sender,
                Recipient = email.Recipients,
                Cc = email.Cc,
                Subject = email.Subject,
                Body = email.Body,
                EmailType = emailType,
                OrderID = email.OrderId,
                CustomerID = email.CustomerId,
            };

            using (var cxt = DbContextFactory.Create<GovDbEntities>())
            {
                cxt.Email.Add(eEmail);
                cxt.SaveChanges();
            }
        }
    }
}