using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp;

namespace HHTravel.CRM.Booking_Online.Business.DomainService
{
    /// <summary>
    /// OrderItemManager
    /// </summary>
    internal class OrderItemManager
    {
        private IProductService _svrProduct = new ProductServiceImp();
        private Order _order = new Order();

        public OrderItemManager(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException();
            }

            if (order.Product == null)
            {
                throw new NotSupportedException("Product can't is null");
            }

            if (order.OrderItemList == null)
            {
                order.OrderItemList = new List<OrderItem>();
            }

            if (order.Product.TravelType != TravelType.单品游)
            {
                int productId = order.Product.ProductId;
                // todo: 机票（单品游的机票由用户选择）
                var tickets = _svrProduct.GetTickets(productId, true);
                var ticket = tickets.First();
                var orderItem = new OrderItem
                {
                    AdultNum = order.AdultNum,
                    //AdultPrice = 
                    //Amount = 
                    ChildNum = order.ChildNum,
                    //ChildPrice = 
                    DepartureDate = _order.DepartureDate,
                    ProductId = ticket.TicketId
                    //SpecId = 
                };
                order.OrderItemList.Add(orderItem);
                // 酒店
                var hotels = _svrProduct.GetHotels(productId);
                foreach (var hotel in hotels)
                {
                    orderItem = new OrderItem
                    {
                        AdultNum = order.AdultNum,
                        ChildNum = order.ChildNum,
                        DepartureDate = order.DepartureDate,
                        IsHotelStay = order.IsStay,
                        ReturnDate = order.ReturnDate,
                        HotelDays = order.Days, // todo:
                        ProductId = hotel.HotelId
                        //SpecId=
                    };
                    order.OrderItemList.Add(orderItem);
                }
            }

            _order = order;
        }
        /// <summary>
        /// 添加 OrderItem
        /// </summary>
        /// <param name="orderItem"></param>
        public virtual void AddOrderItem(OrderItem orderItem)
        {
            if (orderItem.ProductId == default(int))
            {
                throw new NotSupportedException("ProductId must be provide. ");
            }

            _order.OrderItemList.Add(orderItem);
        }
    }
}
