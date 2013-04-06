using System;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step1PostModel : IPostModel
    {
        public Guid SessionId { get; set; }

        public int ProductId { get; set; }

        public int? DelayDays { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int AdultCount { get; set; }

        public int? ChildCount { get; set; }

        public void Validate(System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (this.AdultCount == 0)
            {
                modelState.AddModelError("AdultCount", "请选择成人数");
            }
        }
    }
}