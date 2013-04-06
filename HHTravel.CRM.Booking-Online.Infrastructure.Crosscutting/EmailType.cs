﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    [DataContract]
    public enum EmailType
    {
        /// <summary>
        /// 转寄好友
        /// </summary>
        [DataMember]
        Share,

        /// <summary>
        /// 订单
        /// </summary>
        [DataMember]
        Order,

        /// <summary>
        /// 电子报订阅通知
        /// </summary>
        [DataMember]
        NewsSub,
    }
}