using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HHTravel.CRM.Booking_Online.Model
{
    [DataContract]
    public class Email
    {
        /// <summary>
        /// 发件人
        /// </summary>
        [DataMember]
        public string Sender { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        [DataMember]
        public string Recipients { get; set; }
        /// <summary>
        /// 抄送
        /// </summary>
        [DataMember]
        public string Cc { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        [DataMember]
        public string Subject { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [DataMember]
        public string Body { get; set; }

        /// <summary>
        /// 邮件内容类型
        /// Order/Share
        /// </summary>
        [Required]
        [StringLength(20)]
        [DataMember]
        public EmailType EmailType { get; set; }

        /// <summary>
        /// 订单Id (EmailType==Order时必须传）
        /// </summary>
        [DataMember]
        public int? OrderId { get; set; }
        /// <summary>
        /// Customer Id (EmailType==Order时必须传）
        /// </summary>
        [DataMember]
        public int? CustomerId { get; set; }
    }
}
