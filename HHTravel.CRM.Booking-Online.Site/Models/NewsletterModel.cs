using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Site.Models
{
    public class NewsletterModel
    {
        // 默认打开视图选中“订阅”
        private bool isSubscription = true;

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "请输入您的E-mail")]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件格式错误")]
        public string Email { get; set; }
        [Required(ErrorMessage = "请再输入一次E-mail")]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件格式错误")]
        public string EmailAgain { get; set; }
        [UIHint("_IsSubscription")]
        [Required(ErrorMessage = "请选择订阅还是取消")]
        public bool IsSubscription
        {
            get { return isSubscription; }
            set { isSubscription = value; }
        }

        [Required(ErrorMessage = "请输入验证码")]
        [StringLength(6)]
        [RegularExpression(@"^[\d\S]{6}$", ErrorMessage = "验证码格式不正确")]
        public string Captcha { get; set; }
    }
}