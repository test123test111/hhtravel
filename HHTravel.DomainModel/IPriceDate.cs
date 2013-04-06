using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    internal interface IPriceDate
    {
        PriceDate GetPriceDate(DateTime date);
    }
}