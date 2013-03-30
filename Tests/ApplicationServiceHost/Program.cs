using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HHTravel.CRM.Booking_Online.Business;
using System.ServiceModel;
using HHTravel.CRM.Booking_Online.Business.ApplicationServiceImp;
using HHTravel.CRM.Booking_Online.Business.IApplicationService;
using System.ServiceModel.Description;

namespace ApplicationServiceHost
{
    class Program
    {
        static void Main(string[] args)
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
