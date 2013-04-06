using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HHTravel.CRM.Booking_Online.Infrastructure.Web.Mvc;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class RoomClassModel
    {
        public string BreakfastDinnerTip { get; set; }

        public int Count { get; set; }

        public bool WithLowestPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal SegmentPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal SegmentPriceDelta { get; set; }

        public int RoomClassId { get; set; }

        public string RoomClassName { get; set; }

        public int? StayCount { get; set; }

        public decimal? StayPrice { get; set; }

        public int MaxOccupancy { get; set; }

        public SelectList GetSelectList(int selectedValue = 0, int endValue = 8, string textFormat = "{0}间")
        {
            return SelectListFactory.CreateSelectList(textFormat, selectedValue, endValue: endValue);
        }
    }
}