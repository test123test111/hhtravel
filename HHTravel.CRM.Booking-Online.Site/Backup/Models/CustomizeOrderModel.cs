using System;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class CustomizeOrderModel
    {
        /// <summary>
        /// 天数
        /// </summary>
        [Required(ErrorMessage = "请填写旅行天数")]
        [RegularExpression(@"[\d]+", ErrorMessage = "请填写旅行天数")]
        public short? Days { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Required(ErrorMessage = "产品名称缺失")]
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [Required(ErrorMessage = "产品编号缺失")]
        public string ProductNo { get; set; }

        #region 用户输入部分

        /// <summary>
        /// 参加人数（成人）
        /// </summary>
        [RegularExpression(@"^[\d]+$", ErrorMessage = "请输入参加人数")]
        public short AdultNum { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        [StringLength(6, ErrorMessage = "长度超过限制")]
        [RegularExpression(@"^[\d\S]{6}$", ErrorMessage = "请输入验证码")]
        public string Captcha { get; set; }

        /// <summary>
        /// 参加人数（孩童）
        /// </summary>
        [RegularExpression(@"^[\d]+$", ErrorMessage = "请输入参加人数")]
        public short ChildNum { get; set; }

        /// <summary>
        /// 联络方式偏好
        /// 0: phone, 1: email
        /// </summary>
        [UIHint("_ContactFavorite")]
        [Required(ErrorMessage = "请选择联络方式偏好")]
        public int ContactFavorite { get; set; }

        /// <summary>
        /// 方便接听电话的时段
        /// </summary>
        [UIHint("_ConvinientTime")]
        [Required(ErrorMessage = "请选择方便接听电话时间")]
        public int ConvinientTime { get; set; }

        /// <summary>
        /// 出发日期
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:yyyy年MM月dd日}")]

        //[Date(ErrorMessage = "错误的日期格式")]
        //[DataType(DataType.Date, ErrorMessage = "错误的日期格式2")]
        [Required(ErrorMessage = "请输入出发日期")]
        public DateTime? DepartureDate { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataType(DataType.EmailAddress, ErrorMessage = "电子邮件格式错误2")]
        [Required(ErrorMessage = "请输入E-mail")]
        [StringLength(50, ErrorMessage = "长度超过限制")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件格式错误")]
        public string Email { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        [Required(ErrorMessage = "请输入中文姓")]
        [StringLength(10, ErrorMessage = "长度超过限制")]
        public string FirstName { get; set; }

        /// <summary>
        /// 名（英文）
        /// </summary>
        [StringLength(50, ErrorMessage = "长度超过限制")]
        public string FirstNameEn { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [Required(ErrorMessage = "请输入中文名")]
        [StringLength(10, ErrorMessage = "长度超过限制")]
        public string LastName { get; set; }

        /// <summary>
        /// 姓（英文）
        /// </summary>
        [StringLength(50, ErrorMessage = "长度超过限制")]
        public string LastNameEn { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DataType(DataType.PhoneNumber, ErrorMessage = "电话号码格式错误2")]
        [Required(ErrorMessage = "请输入联络电话")]
        [StringLength(20, ErrorMessage = "长度超过限制")]

        //[RegularExpression(@"(\d{4,14}\b)", ErrorMessage = "电话号码格式错误")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 想避免在行程里的城市、景点、活动、餐厅
        /// </summary>
        [StringLength(200, ErrorMessage = "长度超过限制")]
        public string SpecialHope { get; set; }

        /// <summary>
        /// 这趟旅行有特殊纪念意义或庆祝特别的日子吗？
        /// </summary>
        [StringLength(200, ErrorMessage = "长度超过限制")]
        public string SpecialMemento { get; set; }

        #endregion 用户输入部分

        /// <summary>
        /// 请求来源
        /// </summary>
        public string RequestFrom { get; set; }

        /// <summary>
        /// 是否用于发送邮件
        /// </summary>
        public bool UsedByEmail { get; set; }
    }
}