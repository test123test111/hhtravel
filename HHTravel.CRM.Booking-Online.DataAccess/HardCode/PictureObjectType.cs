using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HHTravel.CRM.Booking_Online.DataAccess.HardCode
{
    /// <summary>
    /// 对应Picture表中的ObjectType
    /// todo: 从配置源读取，自动生成
    /// </summary>
    internal struct PictureObjectType
    {
        /// <summary>
        /// 属性
        /// </summary>
        public const string 属性 = "属性";
        /// <summary>
        /// 产品
        /// </summary>
        public const string 产品 = "产品";
        /// <summary>
        /// 产品日程
        /// </summary>
        public const string 产品日程 = "产品日程";
        /// <summary>
        /// 产品行程
        /// </summary>
        public const string 产品行程 = "产品行程";
        /// <summary>
        /// 产品规格
        /// </summary>
        public const string 产品规格 = "产品规格";
    }
}
