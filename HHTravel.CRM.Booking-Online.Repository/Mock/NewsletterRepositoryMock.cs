namespace HHTravel.CRM.Booking_Online.Repository.Mock
{
    public class NewsletterRepositoryMock : NewsletterRepository
    {
        public override void Subscribe(string email)
        {
            System.Diagnostics.Debugger.Break();

            // do nothing!
        }
    }
}