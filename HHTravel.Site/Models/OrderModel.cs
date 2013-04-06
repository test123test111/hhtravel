using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using HHTravel.Infrastructure;
using HHTravel.Infrastructure.Crosscutting;
using HHTravel.Site.Controllers;

namespace HHTravel.Site.Models
{
    public class CustomerModel
    {
        /// <summary>
        /// 姓
        /// </summary>
        //[Required(ErrorMessage = "请输入中文姓")]
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
        //[Required(ErrorMessage = "请输入中文名")]
        [StringLength(10, ErrorMessage = "长度超过限制")]
        public string LastName { get; set; }

        /// <summary>
        /// 姓（英文）
        /// </summary>
        [StringLength(50, ErrorMessage = "长度超过限制")]
        public string LastNameEn { get; set; }
    }

    public class OrderModel : IPostModel
    {
        public OrderModel()
        {
            this.OtherCustomerModelList = new List<CustomerModel>();
            this.HotelSegmentModelList = new List<HotelSegmentModel>();
        }

        /// <summary>
        /// 订单金额
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public decimal Amount { get; set; }

        public BasicInfoModel BasicInfoModel { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        [StringLength(6, ErrorMessage = "长度超过限制")]
        [RegularExpression(@"^[\d\S]{6}$", ErrorMessage = "验证码格式不正确")]
        public string Captcha { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }

        public List<CustomerModel> OtherCustomerModelList { get; set; }

        public PreOrderModel PreOrderModel { get; set; }

        /// <summary>
        /// 请求来源
        /// </summary>
        public string RequestFrom { get; set; }

        #region TravelType2

        public List<HotelSegmentModel> HotelSegmentModelList { get; set; }

        public TicketModel SelectedTicketModel { get; set; }

        #endregion TravelType2

        public StayInfoModel StayInfoModel { get; set; }

        /// <summary>
        /// 是否用于发送邮件
        /// </summary>
        public bool UsedByEmail { get; set; }

        public virtual void Validate(ModelStateDictionary modelState)
        {
            if (!CaptchaController.Validate(this.Captcha))
            {
                modelState.AddModelError("Captcha", "验证码错误");
            }

            foreach (var cm in this.OtherCustomerModelList)
            {
                if (!string.IsNullOrEmpty(cm.FirstName) && string.IsNullOrEmpty(cm.LastName))
                {
                    modelState.AddModelError("OtherCustomerModelList", "请输入随行旅客的完整的姓名");
                }
                else if (string.IsNullOrEmpty(cm.FirstName) && !string.IsNullOrEmpty(cm.LastName))
                {
                    modelState.AddModelError("OtherCustomerModelList", "请输入随行旅客的完整的姓名");
                }
            }
        }
    }
}