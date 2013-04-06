using System.ServiceModel;
using HHTravel.IRepository;

namespace HHTravel.ApplicationService.ApplicationServiceImp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class OrderServiceImp : IOrderService
    {
        /// <summary>
        /// 插入订单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public DomainModel.Order Add(DomainModel.Order order)
        {
            RepositoryCaller.Call<IOrderRepository>((repo) =>
            {
                repo.Add(order);
            });
            return order;
        }
    }
}