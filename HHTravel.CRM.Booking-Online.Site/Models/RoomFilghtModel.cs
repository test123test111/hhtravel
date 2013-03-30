using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HHTravel.CRM.Booking_Online.Model;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class RoomFilghtModel
    {
        public string ProductNo { get; set; }
        /// <summary>
        /// 出发日期；(单品游）是入住日期 CheckInDate
        /// </summary>
        public DateTime DepartureDate { get; set; }
        /// <summary>
        /// 房型信息
        /// </summary>
        public List<RoomClass> RoomClassList { get; set; }
        /// <summary>
        /// 航班
        /// todo: rename
        /// </summary>
        public List<Ticket> TicketList { get; set; }
        /// <summary>
        /// 用于返回上一步
        /// todo: 考虑使用别的机制，如cookie/session 毕竟这个与View无关
        /// </summary>
        public int Year { get; set; }
        public int Month { get; set; }
        /// <summary>
        /// 最大成行人数
        /// </summary>
        public int? MaxPersonNum { get; set; }

        /// <summary>
        /// 回程日期； (单品游）退房日期
        /// </summary>
        public DateTime ReturnDate { get; set; }
        /// <summary>
        /// 延迟退房日期
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "错误的日期格式")]
        [Date(ErrorMessage = "错误的日期格式")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime? StayReturnDate { get; set; }
        /// <summary>
        /// (单品游）住几晚
        /// </summary>
        public int NightCount { get; set; }
    }
}