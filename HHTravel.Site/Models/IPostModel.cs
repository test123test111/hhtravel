using System;

namespace HHTravel.Site.Models
{
    public interface IPostModel
    {
        void Validate(System.Web.Mvc.ModelStateDictionary modelState);
    }
}