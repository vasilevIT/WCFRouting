using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;

namespace Router
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Router";
           ServiceHost host = new ServiceHost(typeof(RoutingService));

           try
           {
               host.Open();
               PrintServiceInfo(host);
               Console.ReadLine();
               host.Close();
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
               host.Abort();
           }
        }
          static void PrintServiceInfo(ServiceHost host)
        {
            Console.WriteLine("{0} is up and running with following endpoint(s)-", host.Description.ServiceType);

            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
                Console.WriteLine("\nA-> {0}, B-> {1}, C-> {2}\n",
                    endpoint.Address,
                    endpoint.Binding.Name,
                    endpoint.Contract.Name);
        }
    }
}
