using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Routing;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace testWCF
{
    public class Program
    {
        public static Notification nt;
        public static bool isRouter = false;
        static void Main(string[] args)
        {
            Console.Title = "Server";
          /*  Uri address = new Uri("http://127.0.0.1:4000/A");
            BasicHttpBinding binding = new BasicHttpBinding();
            Type contract = typeof(IInterface2);
*/
            ServiceHost host = new ServiceHost(typeof(Service));
//            host.AddServiceEndpoint(contract, binding, address);
            host.Open();
            
            ServiceHost routing_host = new ServiceHost(typeof(RoutingService));

            try
            {
                routing_host.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                routing_host.Abort();
                Console.ReadKey();
            }
 
            EndpointAddress address = routing_host.Description.Endpoints[0].Address;

            PerfomanceData pr = new PerfomanceData();
            pr.Initilization(address);
            nt = new Notification(address, pr);
            nt.Run();
            
            Console.WriteLine("Сервер запущен. Нажмите любую клавишу для закрытия.");
            Console.ReadLine();
           // SendCurrentState();
            Console.ReadKey();
 
            host.Close();

        }
        /*
        private static void SendCurrentState()
        {
            Uri address = new Uri("http://localhost:4000/RouterStaff");
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);


            ChannelFactory<IRouterService> factory = new ChannelFactory<IRouterService>(binding, endpoint);
            IRouterService proxy = factory.CreateChannel();
            proxy.SendData(5, 999, 1);
        }
        */
    }
}
