using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HHTravel.Site.Models.OrderWizard
{
    public class GroundServiceModel
    {
        public int GroundServiceId { get; set; }

        public string ServiceName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal AdultUnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ChildUnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal TotalPriceDelta { get; set; }

        public int PurchaseCount { get; set; }

        public bool Checked { get; set; }

        public string Description { get; set; }
    }
}