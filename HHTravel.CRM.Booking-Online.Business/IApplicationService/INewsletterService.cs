using System;
using System.ServiceModel;

namespace HHTravel.CRM.Booking_Online.Business.IApplicationService
{
    [ServiceContract]
    public interface INewsletterService
    {
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
