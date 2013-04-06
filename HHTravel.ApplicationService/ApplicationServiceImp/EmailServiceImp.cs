using HHTravel.DataService;
using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService.ApplicationServiceImp
{
    public class EmailServiceImp : IEmailService
    {
        public void Add(Email email)
        {
            EmailService.Add(email);
        }
    }
}