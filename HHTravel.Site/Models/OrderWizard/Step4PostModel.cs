using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step4PostModel : IPostModel
    {
        public Guid SessionId { get; set; }

        public BasicInfoModel BasicInfoModel { get; set; }

        public List<PassengerModel> PassengerModelList { get; set; }

        public string SpecialRequirement { get; set; }

        public void Validate(System.Web.Mvc.ModelStateDictionary modelState)
        {
            
        }
    }
}