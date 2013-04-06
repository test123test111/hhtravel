using System.ServiceModel;

namespace HHTravel.CRM.Booking_Online.ApplicationService
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