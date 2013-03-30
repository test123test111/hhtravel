using System;
using System.ServiceModel;
namespace HHTravel.CRM.Booking_Online.Business.IApplicationService
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
