using System;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.FlightsPlanService
{
    public interface IFlightsPlanService
    {
        FlightsSegment Search(FlightsPlanSearchCondition sc);
    }
}