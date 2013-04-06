using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class SelectedRoomClassModel : RoomClass
    {
        /// <summary>
        /// 间数
        /// </summary>
        public short RoomCount {get;set;}
        /// <summary>
        /// 延住间数
        /// </summary>
        public short StayCount { get; set; }
    }
}