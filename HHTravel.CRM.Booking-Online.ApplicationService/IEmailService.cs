using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.ApplicationService
{
    public interface IEmailService
    {
        void Add(Email email);
    }
}