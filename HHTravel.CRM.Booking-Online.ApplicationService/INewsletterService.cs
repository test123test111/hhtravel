using System.ServiceModel;
using HHTravel.CRM.Booking_Online.DomainModel;

namespace HHTravel.CRM.Booking_Online.ApplicationService
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