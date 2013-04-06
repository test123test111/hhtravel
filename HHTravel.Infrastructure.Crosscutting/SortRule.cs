using System.Runtime.Serialization;

namespace HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting
{
    [DataContract]
    public enum SortRule
    {
        /// <summary>
        /// 按行程天数
        /// </summary>
        [DataMember]
        ProductTripDays = 1,

        /// <summary>
        /// 按最早出发日期
        /// </summary>
        [DataMember]
        ProductMinDepartureDate = 2,

        /// <summary>
        /// 按最低价格
        /// </summary>
        [DataMember]
        ProductMinPrice = 3,

        /// <summary>
        /// 默认排序规则: 推荐等级(降)+出发日期(升)+天数(升)
        /// </summary>
        [DataMember]
        ProductDefault = 0,
    }
}