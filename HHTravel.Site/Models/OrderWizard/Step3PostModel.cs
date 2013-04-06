using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.Site.Models.OrderWizard
{
    public class Step3PostModel : IPostModel
    {
        public Guid SessionId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalPrice { get; set; }

        public List<string> FlightNos { get; set; }

        public List<string> FlightsSegmentPlanJsons { get; set; }

        public void Validate(System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (FlightNos == null || FlightNos.Count == 0)
            {
                modelState.AddModelError("FlightNos", "请选择去程航班");
            }
            else if (FlightNos.Count < 2)
            {
                modelState.AddModelError("FlightNos", "请选择回程航班");
            }
        }
    }
}