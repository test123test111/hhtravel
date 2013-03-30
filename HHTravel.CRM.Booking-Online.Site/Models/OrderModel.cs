using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class OrderModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        [Required]
        public int ProductId { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        public string ProductName { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        [Required]
        public short Days { get; set; }
        /// <summary>
        /// （单品游）几晚
        /// </summary>
        [Required]
        public short? NightDays { get; set; }
        /// <summary>
        /// 出发日期
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [Date(ErrorMessage = "错误的日期格式")]
        [DataType(DataType.Date, ErrorMessage = "错误的日期格式2")]
        [Required]
        public DateTime DepartureDate { get; set; }
        /// <summary>
        /// 回程日期
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]
        [Date(ErrorMessage = "错误的日期格式")]
        [DataType(DataType.Date, ErrorMessage = "错误的日期格式2")]
        [Required]
        public DateTime ReturnDate { get; set; }

        /// <summary>
        /// (单品游)详细行程
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// (单品游)房型
        /// </summary>
        public List<SelectedRoomClassModel> SelectedRoomClasses { get; set; }
        /// <summary>
        /// (单品游)延住房型
        /// </summary>
        public List<SelectedRoomClassModel> StayRoomClasses { get; set; }
        /// <summary>
        /// (单品游)加购机票
        /// </summary>
        public List<SelectedTicketModel> SelectedTickets { get; set; }

        #region 用户输入部分

        /// <summary>
        /// 是否延住
        /// </summary>
        [UIHint("_Boolean")]
        public bool? IsStay { get; set; }
        /// <summary>
        /// 延迟回程日期; 单品游是延住退房日期; 各旅游型态下，“量身定做”时是自定义回程日期
        /// </summary>
        [Date(ErrorMessage = "错误的日期格式")]
        [DataType(DataType.Date, ErrorMessage = "错误的日期格式2")]
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}", ApplyFormatInEditMode = true)]
        public DateTime? StayReturnDate { get; set; }
        /// <summary>
        /// 机票自理
        /// </summary>
        [UIHint("_Boolean")]
        public bool? AirTicketOneself { get; set; }
        /// <summary>
        /// 延长行程说明
        /// </summary>
        public string StayNote { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        [Required(ErrorMessage = "请输入中文姓")]
        [StringLength(10)]
        public string Suranme { get; set; }
        /// <summary>
        /// 名
        /// </summary>
        [Required(ErrorMessage = "请输入中文名")]
        [StringLength(10)]
        public string FirstName { get; set; }
        /// <summary>
        /// 姓（英文）
        /// </summary>
        [StringLength(50)]
        public string SurnameEn { get; set; }
        /// <summary>
        /// 名（英文）
        /// </summary>
        [StringLength(50)]
        public string FirstNameEn { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        [Required(ErrorMessage = "请输入联络电话")]
        [StringLength(20, ErrorMessage = "电话号码格式错误")]
        [RegularExpression(@"\d{4,14}$", ErrorMessage = "电话号码格式错误")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "请输入E-mail")]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件格式错误")]
        public string Email { get; set; }
        /// <summary>
        /// 参加人数（成人）
        /// </summary>
        public short? AdultNum { get; set; }
        /// <summary>
        /// 参加人数（孩童）
        /// </summary>
        public short? ChildNum { get; set; }
        /// <summary>
        /// 联络方式偏好
        /// 0: phone, 1: email
        /// </summary>
        [Required(ErrorMessage = "请选择联络方式偏好")]
        //[StringLength(50)]
        [UIHint("_ContactFavorite")]
        public int? ContactFavorite { get; set; }
        /// <summary>
        /// 方便接听电话的时段
        /// </summary>
        [Required(ErrorMessage = "请选择方便接听电话时间")]
        //[StringLength(50)]
        [UIHint("_ConvinientTime")]
        public int? ConvinientTime { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        [StringLength(6)]
        [RegularExpression(@"^[\d\S]{6}$", ErrorMessage = "验证码格式不正确")]
        public string Captcha { get; set; }

        #endregion

        /// <summary>
        /// 请求来源
        /// </summary>
        public string RequestFrom { get; set; }
    }
}