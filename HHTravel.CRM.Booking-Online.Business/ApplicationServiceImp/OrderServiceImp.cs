using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Business.DomainService;

namespace HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class OrderServiceImp : IOrderService
    {
        private IOrderRepository _repoOrder;

        public IOrderRepository OrderRepository
        {
            get
            {
                if (_repoOrder == null)
                    _repoOrder = RepositoryFactory.GetRepository<IOrderRepository>();
                return _repoOrder;
            }
        }
        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public Model.Order Insert(Model.Order order)
        {
            this.Insert(order, null);
            return order;
        }

        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="additionalOrderItems"></param>
        /// <returns></returns>
        public Model.Order Insert(Model.Order order, IList<Model.OrderItem> additionalOrderItems)
        {
            OrderItemManager manager = new OrderItemManager(order);
            if (additionalOrderItems != null)
            {
                foreach (var orderItem in additionalOrderItems)
                {
                    manager.AddOrderItem(orderItem);
                }
            }

            this.OrderRepository.Insert(order);
            return order;
        }
    }
}
