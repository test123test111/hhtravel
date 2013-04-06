using System.Collections.Generic;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Infrastructure.Web.Mvc;

namespace DataValidator.Models
{
    public class DbServerSelectListFactory
    {
        private static Dictionary<string, string> s_dict = new Dictionary<string, string> {
            {"LOCAL", "LOCAL"},
            {"DEV", "DEV"},
            {"TEST", "TEST"},
            {"PROD TEST", "PROD TEST"},
            {"PROD", "PROD"},
        };

        public static IEnumerable<SelectListItem> Create()
        {
            return SelectListFactory.Create(s_dict, "Value", "Key");
        }
    }
}