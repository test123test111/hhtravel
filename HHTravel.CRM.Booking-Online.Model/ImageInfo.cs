using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    [DataContract]
    public class ImageInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// alt
        /// </summary>
        [DataMember]
        public string Alt { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [DataMember]
        public string Url { get; set; }
    }
}
