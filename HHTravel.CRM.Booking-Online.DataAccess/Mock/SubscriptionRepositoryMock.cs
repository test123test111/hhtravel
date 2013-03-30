using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.DataAccess.Repository;

namespace HHTravel.CRM.Booking_Online.DataAccess.Mock
{
    public class SubscriptionRepositoryMock : NewsletterRepository
    {
        public override void Subscribe(string email)
        {
            System.Diagnostics.Debugger.Break();
            // do nothing!
        }
    }
}
