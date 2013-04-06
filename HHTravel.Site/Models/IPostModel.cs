using System;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public interface IPostModel
    {
        void Validate(System.Web.Mvc.ModelStateDictionary modelState);
    }
}