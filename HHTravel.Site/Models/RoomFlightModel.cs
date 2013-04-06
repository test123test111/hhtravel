using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HHTravel.CRM.Booking_Online.Infrastructure;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class RoomFlightModel
    {
        public RoomFlightModel()
        {
            RoomClassModelList = new List<RoomClassModel>();
            TicketModelList = new List<TicketModel>();
            HotelSegmentModelList = new List<HotelSegmentModel>();
        }

        /// <summary>
        /// 天数
        /// </summary>
        public int Days { get; set; }

        /// <summary>
        /// 出发日期；(TravelType3）是入住日期 CheckInDate
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// 最大成行人数
        /// </summary>
        public int? MaxPersonNum { get; set; }

        /// <summary>
        /// 最小成行人数
        /// </summary>
        public int? MinPersonNum { get; set; }

        public int Month { get; set; }

        public PreOrderModel PreOrderModel { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        /// <summary>
        /// 回程日期； (TravelType3）退房日期
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// 用于返回上一步
        /// todo: 考虑使用别的机制，如cookie/session 毕竟这个与View无关
        /// </summary>
        public int Year { get; set; }

        #region TravelType1/4

        /// <summary>
        /// 房型信息
        /// </summary>
        public List<RoomClassModel> RoomClassModelList { get; set; }

        /// <summary>
        /// 机票
        /// </summary>
        public List<TicketModel> TicketModelList { get; set; }

        #endregion TravelType1/4

        #region TravelType3

        /// <summary>
        /// （TravelType3）夜数
        /// </summary>
        public short? Nights { get; set; }

        /// <summary>
        /// 延迟退房日期
        /// </summary>
        [DataType(DataType.Date, ErrorMessage = "错误的日期格式")]
        [Date(ErrorMessage = "错误的日期格式")]
        [DisplayFormat(DataFormatString = "yyyy-MM-dd", ApplyFormatInEditMode = true)]
        public DateTime? StayReturnDate { get; set; }

        #endregion TravelType3

        #region TravelType2

        /// <summary>
        /// 酒店行程段
        /// </summary>
        public List<HotelSegmentModel> HotelSegmentModelList { get; set; }

        /// <summary>
        /// 产品价格
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 机票行程段
        /// </summary>
        public TicketSegmentModel TicketSegmentModel { get; set; }

        #endregion TravelType2
    }
}