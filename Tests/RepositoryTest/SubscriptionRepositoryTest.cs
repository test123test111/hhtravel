using System.Linq;
using HHTravel.CRM.Booking_Online.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RepositoryTest
{
    [TestClass]
    public class SubscriptionRepositoryTest : RepositoryTestBase
    {
        [TestMethod]
        public void AllTest()
        {
            string email = "abc@hhtravel.com";
            var repo = new NewsletterRepository();
            repo.Subscribe(email);

            var a = from s in repo.All()
                    where s.Email == email && s.IsValid
                    select s;

            Assert.IsNotNull(a.Single());

            repo.Unsubscribe(email);
            a = from s in repo.All()
                where s.Email == email && !s.IsValid
                select s;

            Assert.IsNotNull(a.Single());
        }
    }
}