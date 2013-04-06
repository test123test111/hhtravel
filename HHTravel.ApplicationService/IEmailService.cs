using HHTravel.Infrastructure.Crosscutting;

namespace HHTravel.ApplicationService
{
    public interface IEmailService
    {
        void Add(Email email);
    }
}