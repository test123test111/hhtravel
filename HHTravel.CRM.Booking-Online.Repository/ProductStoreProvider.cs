using System;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace HHTravel.CRM.Booking_Online.Repository
{
    internal abstract class ProductStoreProvider : ProviderBase
    {
        public ProductStoreProvider()
        {
        }

        public abstract List<DateTime> GetDepartureDateList();
    }
}