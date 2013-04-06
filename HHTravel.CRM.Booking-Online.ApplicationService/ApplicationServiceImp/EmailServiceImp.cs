using HHTravel.CRM.Booking_Online.DataService;
using HHTravel.CRM.Booking_Online.Infrastructure.Crosscutting;

namespace HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp
{
    public class EmailServiceImp : IEmailService
    {
        public void Add(Email email)
        {
            EmailService.Add(email);
        }
    }
}