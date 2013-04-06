using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HHTravel.DomainModel
{
    internal interface IPriceDate
    {
        PriceDate GetPriceDate(DateTime date);
    }
}