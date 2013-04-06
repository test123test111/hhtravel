using System.ComponentModel.DataAnnotations;
using HHTravel.CRM.Booking_Online.Site.Controllers;
using System;

namespace HHTravel.CRM.Booking_Online.Site.Models.OrderWizard
{
    public class Step5PostModel : IPostModel
    {
        public Guid SessionId { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        [StringLength(6, ErrorMessage = "长度超过限制")]
        [RegularExpression(@"^[\d\S]{6}$", ErrorMessage = "验证码格式不正确")]
        public string Captcha { get; set; }

        public void Validate(System.Web.Mvc.ModelStateDictionary modelState)
        {
            if (!CaptchaController.Validate(this.Captcha))
            {
                modelState.AddModelError("Captcha", "验证码错误");
            }
        }
    }
}