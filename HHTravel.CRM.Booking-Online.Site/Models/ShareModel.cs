using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class ShareModel
    {
        [Required(ErrorMessage = "请输入好友的E-mail")]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件格式错误")]
        public string FriendEmail { get; set; }

        [Required(ErrorMessage = "请输入您的E-mail")]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件格式错误")]
        public string YourEmail { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        /// <summary>
        /// 留言
        /// </summary>
        [StringLength(200)]
        public string Message { get; set; }

        public string ProductNo { get; set; }

        public string ProductName { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        [StringLength(6)]
        [RegularExpression(@"^[\d\S]{6}$", ErrorMessage = "验证码格式不正确")]
        public string Captcha { get; set; }
    }
}