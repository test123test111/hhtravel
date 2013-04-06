using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.FlightsPlanService
{
    public interface IFlightsPlanFilterProvider
    {
        IList<FlightsPlanFilter> GetFilters(int productId);
    }
}