using System;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.ApplicationService.ApplicationServiceImp;

namespace ApplicationServiceHost
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //RepositoryService.Register(true);

            //WSHttpBinding binding = new WSHttpBinding();
            //binding.ReceiveTimeout = new TimeSpan(10, 10, 10);

            try
            {
                ServiceHost host = new ServiceHost(typeof(ProductServiceImp));

                //host.AddServiceEndpoint(typeof(IProductService), binding, "http://127.0.0.1:8080/IProductService");

                //ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                //host.Description.Behaviors.Add(behavior);
                //behavior.HttpGetEnabled = true;

                host.Open();
                Console.WriteLine("service began.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadLine();
        }
    }
}