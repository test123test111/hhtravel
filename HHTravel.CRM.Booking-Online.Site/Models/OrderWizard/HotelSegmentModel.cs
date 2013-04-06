using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHTravel.CRM.Booking_Online.Models.OrderWizard
{
    public class HotelSegmentModel
    {
        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public string BeginDateString { get { return this.BeginDate.ToString("yyyy-MM-dd"); } }

        public string EndDateString { get { return this.EndDate.ToString("yyyy-MM-dd"); } }

        public int Nights { get; set; }

        /// <summary>
        /// 是否是延住段
        /// </summary>
        public bool IsStay { get; set; }

        public List<HotelModel> HotelModels { get; set; }

        public int MinUnitPrice { get; set; }
    }

    public class HotelModel
    {
        public int HotelId { get; set; }

        public string HotelName { get; set; }

        public List<RoomClassModel> RoomClassModels { get; set; }

        public bool Checked { get; set; }
    }

    public class RoomClassModel
    {
        public int RoomClassId { get; set; }

        public string RoomClassName { get; set; }

        public string BreadfirstDinner { get; set; }

        public int UnitPrice { get; set; }

        public int UnitPriceDelta { get; set; }

        public int RoomCount { get; set; }

        public SelectList GetSelectList(int i)
        {
            return CreateRoomCountSelectList(i);
        }

        public static SelectList CreateRoomCountSelectList(int selectedValue = 0)
        {
            SelectList sl;
            int[] arr = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var q = from i in arr
                    select new
                    {
                        Id = i,
                        Name = string.Format("{0}间", i)
                    };
            var list = new SelectList(q, "Id", "Name");
            sl = new SelectList(list, "Value", "Text", selectedValue);
            return sl;
        }
    }
}