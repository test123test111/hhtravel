using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HHTravel.CRM.Booking_Online.Site.Models.OrderWizard
{
    public class PassengerModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(10, ErrorMessage = "长度超过限制")]
        public string Name { get; set; }

        /// <summary>
        /// 姓名（英文）
        /// </summary>
        [StringLength(50, ErrorMessage = "长度超过限制")]
        public string NameEn { get; set; }

        /// <summary>
        /// 0：身份证 1：护照
        /// </summary>
        public int IdentifierType { get; set; }

        public string IdentifierNo { get; set; }
    }
}