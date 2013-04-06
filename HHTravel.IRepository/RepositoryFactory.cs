using System;
using Microsoft.Practices.Unity;

namespace HHTravel.IRepository
{
    public static class RepositoryFactory
    {
        private static IUnityContainer s_container = new UnityContainer();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static void RegisterType<TFromRepository, TToRepository>()
            where TToRepository : class, TFromRepository
        {
            s_container.RegisterType<TFromRepository, TToRepository>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public static void RegisterType(Type fromType, Type toType)
        {
            s_container.RegisterType(fromType, toType);
        }

        internal static TRepository GetRepository<TRepository>()
            where TRepository : class
        {
            TRepository repository = default(TRepository);
            repository = s_container.Resolve<TRepository>();

            return repository;
        }
    }
}