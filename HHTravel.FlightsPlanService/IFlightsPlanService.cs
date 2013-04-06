using System;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.FlightsPlanService
{
    public interface IFlightsPlanService
    {
        FlightsSegment Search(FlightsPlanSearchCondition sc);
    }
}