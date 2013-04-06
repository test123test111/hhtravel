using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HHTravel.Site.Models
{
    public class BasicInfoModel
    {
        /// <summary>
        /// 联络方式偏好
        /// </summary>
        //[StringLength(50, ErrorMessage = "长度超过限制")]
        [Required(ErrorMessage = "请选择联络方式偏好")]
        [UIHint("_ContactFavorite")]
        public int ContactFavorite { get; set; }

        /// <summary>
        /// 方便接听电话的时段
        /// </summary>
        //[StringLength(50, ErrorMessage = "长度超过限制")]
        [Required(ErrorMessage = "请选择方便接听电话时间")]
        [UIHint("_ConvinientTime")]
        public int ConvinientTime { get; set; }

        /// <summary>
        /// Email
        /// </summary>
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
        //[RegularExpression(@"(?:\b\d{4,14}\b)", ErrorMessage = "电话号码格式错误")]
        [Required(ErrorMessage = "请输入联络电话")]
        [StringLength(20, ErrorMessage = "长度超过限制")]
        public string PhoneNumber { get; set; }

        public void Validate(System.Web.Mvc.ModelStateDictionary modelState)
        {
        }
    }
}