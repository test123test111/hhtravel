using System.ServiceModel;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using HHTravel.CRM.Booking_Online.IRepository;
using HHTravel.CRM.Booking_Online.Model;

namespace HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class NewsletterServiceImp : INewsletterService
    {
        private static INewsletterRepository s_repo;

        static NewsletterServiceImp()
        {
            s_repo = RepositoryFactory.GetRepository<INewsletterRepository>();
        }

        public NewsletterServiceImp()
        {

        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="email"></param>
        public void Subscribe(string email)
        {
            s_repo.Subscribe(email);
        }

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="email"></param>
        public void Unsubscribe(string email)
        {
            s_repo.Subscribe(email);
        }
    }
}
