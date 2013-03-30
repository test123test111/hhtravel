using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Model;
using Microsoft.Practices.Unity;

namespace HHTravel.CRM.Booking_Online.IRepository
{
    public static class RepositoryFactory
    {
        private static IUnityContainer s_container = new UnityContainer();

        public static void RegisterType<TFromRepository, TToRepository>()
            where TToRepository : class, TFromRepository
        {
            s_container.RegisterType<TFromRepository, TToRepository>(new ContainerControlledLifetimeManager());
        }

        public static void RegisterType(Type tFrom, Type tTo)
        {
            s_container.RegisterType(tFrom, tTo, new ContainerControlledLifetimeManager());
        }

        public static TRepository GetRepository<TRepository>()
            where TRepository : class
        {
            TRepository repository = default(TRepository);
            repository = s_container.Resolve<TRepository>();

            return repository;
        }
    }


}
