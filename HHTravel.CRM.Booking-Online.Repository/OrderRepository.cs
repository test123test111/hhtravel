using System;
using System.Collections.Generic;
using System.Linq;
using HHTravel.CRM.Booking_Online.DataAccess;
using HHTravel.CRM.Booking_Online.DataAccess.DbContexts;
using HHTravel.CRM.Booking_Online.DataAccess.Providers;
using HHTravel.CRM.Booking_Online.DomainModel;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;
using HHTravel.CRM.Booking_Online.IRepository;
using Microsoft.Practices.Unity;

namespace HHTravel.CRM.Booking_Online.Repository
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), InjectionConstructor]
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

        public virtual Order Add(Order order)
        {
            var ordersProvider = new OrdersProvider(this.OrderDbContext);
            var ordersProductsProvider = new OrderProductsProvider(this.OrderDbContext);
            var customersProvider = new CustomersProvider(this.CustomerDbContext);

            var customerEntity = new Entity.Customer
            {
                CnFirstName = order.FirstName,
                CnLastName = order.LastName,
                CustomerName = order.FirstName + order.LastName, // !compute

                //Company =
                ContactFavorite = order.ContactFavorite.GetText(),
                ConvenientTime = order.ConvinientTime.GetText(),

                // 自动生成 CustomerId
                Email = order.Email,
                EnFirstName = order.FirstNameEn,
                EnLastName = order.LastNameEn,

                //Remark =
                Telphone = order.PhoneNumber,
            };
            customersProvider.Insert(customerEntity);

            var orderEntity = new Entity.Orders
            {
                AdultNum = order.AdultNum,
                AirTicketOneself = order.AirTicketOneself,
                Amount = (int)order.Amount,
                ChildNum = order.ChildNum,
                PersonNum = (short)(order.AdultNum + order.ChildNum), // !compute

                //Company =
                CustomerID = customerEntity.CustomerId,
                DepartDate = order.DepartDate,
                IsStayHotel = order.IsHotelStay,

                RequestFrom = order.RequestFrom,
                ReturnDate = order.PlanReturnDate,
                SpecialHope = order.SpecialHope ?? order.StayNote,
                SpecialMemento = order.SpecialMemento,
                StayReturnDate = order.ActualReturnDate,
                TravelDays = order.Days,
                TripType = (short?)order.Product.TravelType,
                DepartureCity = order.Product.DepartureCity,
                ProductID = (int)order.Product.ProductId,
                
                OrderType = (short)order.OrderType,
            };
            ordersProvider.Insert(orderEntity);
            order.OrderId = orderEntity.OrderId;

            order.CustomerId = customerEntity.CustomerId;

            // 生成订单号并更新回数据库
            orderEntity.OrderNo = NewOrderNo(orderEntity.OrderId, order.Product.DepartureCity, order.Product.TravelType);
            ordersProvider.Update(orderEntity);
            order.OrderNo = orderEntity.OrderNo;

            var orderProductEntityList = from item in order.OrderItemList
                                         select new Entity.Orders_Product
                                         {
                                             Quantity = (short)item.Quantity,
                                             Amount = (int)item.Amount,
                                             DepartDate = item.BeginDate,
                                             ReturnDate = item.EndDate,
                                             OrderId = orderEntity.OrderId,
                                             Price = (int)item.UnitPrice,
                                             ProductId = item.ProductId,
                                             ProductSpecId = item.SpecId,
                                             Times = (short)item.Times,
                                             ProductName = item.ProductName,
                                             ProductSpecName = item.SpecName,
                                             SectionType = item.ProductType,
                                             Unit = item.Unit,
                                             Remark = item.Remark,
                                         };
            ordersProductsProvider.Insert(orderProductEntityList);

            order.PassengerList.ForEach(p =>
            {
                this.OrderDbContext.Orders_Passenger.Add(new Entity.Orders_Passenger
                {
                    CName = p.FirstName + p.LastName,
                    EName = p.FirstNameEn + p.LastNameEn,
                    OrderId = orderEntity.OrderId,
                });
            });
            this.OrderDbContext.SaveChanges();

            return order;
        }

        public override IEnumerable<Order> All()
        {
            throw new NotImplementedException();
        }

        public override Order Find(int id)
        {
            var ordersProvider = new OrdersProvider(this.OrderDbContext);
            var a = ordersProvider.Find(id);
            return new Order { };
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="departureCtiyCode"></param>
        /// <param name="travelType"></param>
        /// <returns></returns>
        protected string NewOrderNo(int orderId, string departureCtiyCode, TravelType travelType)
        {
            string orderNo;

            string travelTypeCode;
            switch (travelType)
            {
                case TravelType.TravelType1:
                case TravelType.TravelType4:
                    travelTypeCode = "GT";
                    break;

                case TravelType.TravelType2:
                    travelTypeCode = "PT";
                    break;

                case TravelType.TravelType3:
                    travelTypeCode = "FT";
                    break;

                default:
                    throw new ArgumentOutOfRangeException("travelType");
            }

            orderNo = string.Format("{0}{1}{2:D10}", travelTypeCode, departureCtiyCode, orderId);
            return orderNo;
        }
    }
}