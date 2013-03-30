using System;
using System.Collections.Generic;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.TableWorkers;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;
using Microsoft.Practices.Unity;

namespace HHTravel.CRM.Booking_Online.DataAccess.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        [InjectionConstructor]
        public OrderRepository()
        {
            this.OrderDbContext = DbContextFactory.Create<OrderDbEntities>();
            this.CustomerDbContext = DbContextFactory.Create<CustomerDbEntities>();
        }

        internal OrderRepository(OrderDbEntities orderDbContext, CustomerDbEntities customerDbContext)
        {
            this.OrderDbContext = orderDbContext;
            this.CustomerDbContext = customerDbContext;
        }

        public virtual Order Insert(Order order)
        {
            var orderWorker = new OrdersTableWorker(this.OrderDbContext);
            var orderProductWorker = new OrdersProductTableWorker(this.OrderDbContext);
            var customerWorker = new CustomerTableWorker(this.CustomerDbContext);

            var customerEntity = new Entity.Customer
            {
                CnFirstName = order.FirstName,
                CnLastName = order.Suranme,
                //Company =
                ContactFavorite = order.ContactFavorite.ToString(), // todo: enum value to string
                ConvenientTime = order.ConvinientTime.ToString(), // todo: enum value to string
                // 自动生成 CustomerId
                Email = order.Email,
                EnFirstName = order.FirstNameEn,
                EnLastName = order.SurnameEn,
                //Remark =
                Telphone = order.PhoneNumber,
            };
            customerWorker.Insert(customerEntity);

            var orderEntity = new Entity.Orders
            {
                AdultNum = order.AdultNum,
                AirTicketOneself = order.AirTicketOneself,
                // todo: Amount = 订单金额
                ChildNum = order.ChildNum,
                //Company =
                CustomerID = customerEntity.CustomerId,
                DepartureTime = order.DepartureDate,
                IsStayHotel = order.IsStay,
                // 自动生成 OrderDate = Date
                // 自动生成 OrderId = varchar(20)
                // todo: OrderNo
                // todo: OrderState
                // todo: OrderTime = Time(5)
                // todo: PersonNum = Adult + child ?
                RequestFrom = order.RequestFrom,
                ReturnDate = order.ReturnDate,
                SpecialHope = order.SpecialHope,
                //SpecialMemento =
                // todo: StayReturnDate = 延住回程日期
                TravelDays = order.Days,
                TripType = (short?)order.Product.TravelType,
                DepartureCity = order.Product.DepartureCity,
            };
            orderWorker.Insert(orderEntity);
            order.OrderId = orderEntity.OrderId;

            order.CustomerId = customerEntity.CustomerId;
            // 生成订单号并更新回数据库
            orderEntity.OrderNo = NewOrderNo(orderEntity.OrderId, order.Product.DepartureCity, order.Product.TravelType);
            orderWorker.Update(orderEntity);
            order.OrderNo = orderEntity.OrderNo;

            var orderProductEntityList = from item in order.OrderItemList
                                         select new Entity.Orders_Product
                                         {
                                             AdultNum = item.AdultNum,
                                             Amount = item.Amount,
                                             ChildNum = item.ChildNum,
                                             DepartureTime = item.DepartureDate,
                                             IsStay = item.IsHotelStay,
                                             OrderId = orderEntity.OrderId,
                                             PriceAdult = item.AdultPrice,
                                             PriceChild = item.ChildPrice,
                                             ProductId = item.ProductId,
                                             ProductSpecId = item.SpecId,
                                             //ProductState =
                                             ReturnDate = item.ReturnDate,
                                             Times = item.HotelDays,
                                         };
            orderProductWorker.Insert(orderProductEntityList);

            return order;
        }

        public override Order Find(int id)
        {
            var orderWorker = new OrdersTableWorker(this.OrderDbContext);
            var a = orderWorker.Find(id);
            return new Order { };
        }

        public override IEnumerable<Order> All()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="departureCtiy"></param>
        /// <param name="travelType"></param>
        /// <returns></returns>
        protected string NewOrderNo(int orderId, string departureCtiy, TravelType travelType)
        {
            string orderNo;

            string travelTypeCode;
            switch (travelType)
            {
                case TravelType.团队游:
                    travelTypeCode = "TD";
                    break;
                case TravelType.自由行:
                    travelTypeCode = "ZY";
                    break;
                case TravelType.单品游:
                    travelTypeCode = "DP";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("travelType");
            }

            string departureCtiyCode;
            switch (departureCtiy)
            {
                case "北京":
                    departureCtiyCode = "BJ";
                    break;
                case "上海":
                    departureCtiyCode = "SH";
                    break;
                case "成都":
                    departureCtiyCode = "CD";
                    break;
                case "广州":
                    departureCtiyCode = "GZ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("departureCtiy");
            }

            orderNo = string.Format("{0}{1}{2:D10}", travelTypeCode, departureCtiyCode, orderId);
            return orderNo;
        }
    }
}
