using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.DomainModel;

namespace HHTravel.CRM.Booking_Online.IRepository
{
    public interface INewsletterRepository : IRepository<Subscription>
    {
        Subscription Find(string email);

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="email"></param>
        void Subscribe(string email);

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="email"></param>
        void Unsubscribe(string email);
    }
}