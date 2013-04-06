using System.ComponentModel.DataAnnotations;

namespace HHTravel.Site.Models
{
    public class ShareModel
    {
        [Required(ErrorMessage = "请输入验证码")]
        [StringLength(6, ErrorMessage = "长度超过限制")]
        [RegularExpression(@"^[\d\S]{6}$", ErrorMessage = "验证码格式不正确")]
        public string Captcha { get; set; }

        [Required(ErrorMessage = "请输入好友的E-mail")]
        [StringLength(50, ErrorMessage = "长度超过限制")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件格式错误")]
        public string FriendEmail { get; set; }

        /// <summary>
        /// 留言
        /// </summary>
        [StringLength(1024, ErrorMessage = "长度超过限制")]
        public string Message { get; set; }

        public string ProductName { get; set; }

        public string ProductNo { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(200, ErrorMessage = "长度超过限制")]
        public string Title { get; set; }

        [Required(ErrorMessage = "请输入您的E-mail")]
        [StringLength(50, ErrorMessage = "长度超过限制")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "电子邮件格式错误")]
        public string YourEmail { get; set; }
    }
}