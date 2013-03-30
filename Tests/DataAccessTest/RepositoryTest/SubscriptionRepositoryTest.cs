using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HHTravel.CRM.Booking_Online.DataAccess.Repository;
using HHTravel.CRM.Booking_Online.Model;

namespace DataAccessTest.RepositoryTest
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
