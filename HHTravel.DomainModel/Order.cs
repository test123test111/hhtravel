using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.DomainModel
{
    /// <summary>
    /// TravelType3多一个费用包含
    /// </summary>
    [DataContract]
    public class Order : IAggregateRoot
    {
        public Order()
        {
            this.OrderItemList = new List<OrderItem>();
            this.PassengerList = new List<Passenger>();
        }

        /// <summary>
        /// 延长后的返回日期
        /// </summary>
        [DataMember]
        public DateTime? ActualReturnDate { get; set; }

        /// <summary>
        /// 参加人数（成人）
        /// </summary>
        [DataMember]
        public short AdultNum { get; set; }

        /// <summary>
        /// 机票自理
        /// </summary>
        [DataMember]
        public bool? AirTicketOneself { get; set; }

        /// <summary>
        /// 订单金额
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// 参加人数（孩童）
        /// </summary>
        [DataMember]
        public short ChildNum { get; set; }

        /// <summary>
        /// 联络方式偏好
        /// 0: phone, 1: email
        /// </summary>
        [DataMember]
        public ContactFavorite ContactFavorite { get; set; }

        /// <summary>
        /// 方便接听电话的时段
        /// </summary>
        [DataMember]
        public ConvinientTime ConvinientTime { get; set; }

        /// <summary>
        /// CustomerId
        /// </summary>
        [DataMember]
        public int CustomerId { get; set; }

        /// <summary>
        /// 行程天数
        /// </summary>
        [DataMember]
        public short? Days { get; set; }

        /// <summary>
        /// 出发日期
        /// </summary>
        [DataMember]
        public DateTime DepartDate { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// 名（英文）
        /// </summary>
        [DataMember]
        public string FirstNameEn { get; set; }

        /// <summary>
        /// 是否延住
        /// </summary>
        [DataMember]
        public bool IsHotelStay { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// 姓（英文）
        /// </summary>
        [DataMember]
        public string LastNameEn { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        [DataMember]
        public int OrderId { get; set; }

        /// <summary>
        /// 酒店/机票等
        /// </summary>
        [DataMember]
        public List<OrderItem> OrderItemList { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [DataMember]
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单类型-0标准|1定制
        /// </summary>
        [DataMember]
        public int OrderType { get; set; }

        /// <summary>
        /// 随行旅客
        /// </summary>
        [DataMember]
        public List<Passenger> PassengerList { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 计划返程日期
        /// </summary>
        [DataMember]
        public DateTime PlanReturnDate { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        [DataMember]
        public Product Product { get; set; }

        /// <summary>
        /// 请求来源
        /// </summary>
        [DataMember]
        public string RequestFrom { get; set; }

        /// <summary>
        /// （量身定做）这趟旅行有特殊纪念意义或庆祝特别的日子吗？
        /// </summary>
        [DataMember]
        public string SpecialHope { get; set; }

        /// <summary>
        /// （量身定做）想避免在行程里的城市、景点、活动、餐厅
        /// </summary>
        [DataMember]
        public string SpecialMemento { get; set; }

        /// <summary>
        /// 延长行程的其他要求
        /// </summary>
        [DataMember]
        public string StayNote { get; set; }
    }
}