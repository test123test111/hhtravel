using System.ServiceModel;
using HHTravel.DomainModel;
using HHTravel.IRepository;

namespace HHTravel.ApplicationService.ApplicationServiceImp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class NewsletterServiceImp : INewsletterService
    {
        public Subscription Find(string email)
        {
            Subscription sub = null;
            RepositoryCaller.Call<INewsletterRepository>((repo) =>
            {
                sub = repo.Find(email);
            });
            return sub;
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="email"></param>
        public void Subscribe(string email)
        {
            RepositoryCaller.Call<INewsletterRepository>((repo) =>
            {
                repo.Subscribe(email);
            });
        }

        /// <summary>
        /// 退订
        /// </summary>
        /// <param name="email"></param>
        public void Unsubscribe(string email)
        {
            RepositoryCaller.Call<INewsletterRepository>((repo) =>
            {
                repo.Unsubscribe(email);
            });
        }
    }
}