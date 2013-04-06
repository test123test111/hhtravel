using System;
using System.ComponentModel.DataAnnotations;
using HHTravel.CRM.Booking_Online.Infrastructure;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class StayInfoModel
    {
        /// <summary>
        /// 机票自理
        /// </summary>
        [UIHint("_Boolean")]
        public bool? AirTicketOneself { get; set; }

        /// <summary>
        /// 是否延住
        /// </summary>
        [UIHint("_Boolean")]
        public bool? IsHotelStay { get; set; }

        /// <summary>
        /// 延长行程说明
        /// </summary>
        public string StayNote { get; set; }

        /// <summary>
        /// 延迟回程日期; TravelType3是延住退房日期; 各旅游型态下，“量身定做”时是自定义回程日期
        /// </summary>
        [Date(ErrorMessage = "错误的日期格式")]
        [DataType(DataType.Date, ErrorMessage = "错误的日期格式2")]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}", ApplyFormatInEditMode = true)]
        public DateTime? StayReturnDate { get; set; }

        public void Validate(System.Web.Mvc.ModelStateDictionary modelState, DateTime planReturnDate)
        {
            if (this.StayReturnDate.HasValue)
            {
                if (!this.IsHotelStay.HasValue)
                {
                    modelState.AddModelError("StayInfoModel.IsHotelStay", "是否需安排续住");
                }
                if (!this.AirTicketOneself.HasValue)
                {
                    modelState.AddModelError("StayInfoModel.AirTicketOneself", "是否仅参加当地行程，机票自理");
                }

                // 延迟回程日期必须在（出发日期+行程天数）之后
                if (this.StayReturnDate.Value <= planReturnDate)
                {
                    modelState.AddModelError("StayInfoModel.StayReturnDate", "延迟回程日期须在正常回程日期之后");
                }
            }

            if (this.IsHotelStay.HasValue || this.AirTicketOneself.HasValue)
            {
                if (!this.StayReturnDate.HasValue)
                {
                    modelState.AddModelError("StayInfoModel.StayReturnDate", "请选择日期");
                }
            }
        }
    }
}