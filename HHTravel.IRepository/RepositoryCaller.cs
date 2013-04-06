using System;

namespace HHTravel.IRepository
{
    public class RepositoryCaller
    {
        public static void Call<T>(Action<T> action) where T : class, IDisposable
        {
            using (var repo = RepositoryFactory.GetRepository<T>())
            {
                action(repo);
            }
        }
    }
}