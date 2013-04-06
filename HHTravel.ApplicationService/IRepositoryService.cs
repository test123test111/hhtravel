using System.ServiceModel;

namespace HHTravel.ApplicationService
{
    [ServiceContract]
    public interface IRepositoryService
    {
        [OperationContract]
        void Register();

        [OperationContract]
        void Register(bool mock);
    }
}