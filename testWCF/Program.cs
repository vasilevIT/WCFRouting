using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Routing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library;

namespace testWCF
{
    public class Program
    {
        public static Notification nt;
        public static bool isRouter = false;
        public static EndpointAddress address = null;

        public static System.Configuration.Configuration config =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        static void Main(string[] args)
        {

            ThreadPool.SetMaxThreads(Thread.CurrentThread.ManagedThreadId, 10);
           // ThreadPool.SetMinThreads(Thread.CurrentThread.ManagedThreadId, 5);
            System.Net.ServicePointManager.DefaultConnectionLimit = 10;
            Console.Title = "Server";
            ServiceHost host = new ServiceHost(typeof(Service));
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
 
            address = routing_host.Description.Endpoints[0].Address;

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
