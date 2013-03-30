using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Model
{
    [DataContract]
    public class SiteConfig
    {
        /// <summary>
        /// 营销图
        /// todo: 取代以下独立的定义
        /// </summary>
        [Obsolete("未实现", true)]
        public IList<ImageInfo> TopImages { get; set; }

        /// <summary>
        /// 品牌理念营销图
        /// </summary>
        [DataMember]
        public ImageInfo TopImageAboutUs { get; set; }
        /// <summary>
        /// 目的地营销图
        /// </summary>
        [DataMember]
        public ImageInfo TopImageDestination { get; set; }
        /// <summary>
        /// 出发月份营销图
        /// </summary>
        [DataMember]
        public ImageInfo TopImageDeparture { get; set; }
        /// <summary>
        /// 旅行主题营销图
        /// </summary>
        [DataMember]
        public ImageInfo TopImageInterest { get; set; }
        /// <summary>
        /// 热门品鉴营销图
        /// </summary>
        [DataMember]
        public ImageInfo TopImageUnique { get; set; }

        /// <summary>
        /// 环游世界营销图
        /// </summary>
        [DataMember]
        public ImageInfo TopImageAroundWorld { get; set; }
        /// <summary>
        /// 隐私政策
        /// </summary>
        [DataMember]
        public ImageInfo TopImagePrivacyPolicy { get; set; }
        /// <summary>
        /// 电子报订阅
        /// </summary>
        [DataMember]
        public ImageInfo TopImageNewsletter { get; set; }
        /// <summary>
        /// 电子报订阅
        /// </summary>
        [DataMember]
        public ImageInfo TopImageContactUs { get; set; }
        // todo: 
        // 1. 各公共区域的html片段
        // 2. 站点运行参数
        // 3. SEO相关
    }
}
