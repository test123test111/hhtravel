using System.ServiceModel;
using HHTravel.DomainModel;

namespace HHTravel.ApplicationService
{
    [ServiceContract]
    public interface INewsletterService
    {
        [OperationContract]
        Subscription Find(string email);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="email"></param>
        [OperationContract]
        void Subscribe(string email);

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="email"></param>
        [OperationContract]
        void Unsubscribe(string email);
    }
}