using System;
using System.Collections.Generic;
using System.Configuration.Provider;

namespace HHTravel.Repository
{
    internal abstract class ProductStoreProvider : ProviderBase
    {
        public ProductStoreProvider()
        {
        }

        public abstract List<DateTime> GetDepartureDateList();
    }
}