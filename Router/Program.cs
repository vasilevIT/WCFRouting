using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;
using System.Threading;
using Library;

namespace Router
{
    public class Program
    {

        public static Notification nt;
        public static bool isRouter = true;
        public static ConcurrentBag<CustomTask> tasks = new ConcurrentBag<CustomTask>();
        public static ServiceHost host;


        static void Main(string[] args)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 20;
            ThreadPool.SetMaxThreads(Thread.CurrentThread.ManagedThreadId, 20);
            ThreadPool.SetMinThreads(Thread.CurrentThread.ManagedThreadId, 5);

            Console.Title = "Router";

            host = new ServiceHost(typeof(RoutingService));
            ServiceHost host_service = new ServiceHost(typeof(RouterService));
         //   Graphics gr = new Graphics();
          //  gr.ShowDialog();
           try
           {
               host.Open();
               host_service.Open();
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
               host.Abort();
               host_service.Abort();
               Console.ReadKey();
           }
            

            EndpointAddress address = host.Description.Endpoints[0].Address;

            PerfomanceData pr = new PerfomanceData();
            pr.Initilization(address);
            nt = new Notification();
            nt.getListServers().host = host;
            nt.Run();
            /*
            RoutingConfiguration rc = new RoutingConfiguration();
            ServiceEndpoint endpoint = new ServiceEndpoint(
                ContractDescription.GetContract(typeof(IRequestReplyRouter))
                , new BasicHttpBinding()
                , new EndpointAddress("http://192.168.1.68:4000/RouterA3")
                );
            rc.FilterTable.Add(
                new CustomMessageFilter("customGroup_custom"), new List<ServiceEndpoint> { endpoint }
                );
            Program.host.Extensions.Find<RoutingExtension>().ApplyConfiguration(rc);
            */
            PrintServiceInfo(host);
            Console.WriteLine("Сервер запущен. Нажмите любую клавишу для закрытия.");
            Console.ReadKey();
            host.Close();
            host_service.Close();
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

        public static List<CustomTask> getTasksByServer(Uri server, int type_task)
        {
            return Program.tasks.ToList().FindAll(x => x.working_server == server);
        }

        public static void printTasks(List<CustomTask> list)
        {
            Console.WriteLine("Tasks list:");
            list.Sort();
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine("Task #{0}: id: {5}, type: {6}, start: {1}, server: {3}, completed: {4}\n" +
                                  "avg_time: {2}, avg_cpu: {7}, avg_ram: {8}, avr_args: {9}"
                    , i
                    , list.ElementAt(i).start
                    , list.ElementAt(i).taskInfo.average_time
                    , list.ElementAt(i).working_server
                    , list.ElementAt(i).isCompleted()
                    , list.ElementAt(i).Guid
                    , list.ElementAt(i).task_type
                    , list.ElementAt(i).taskInfo.average_cpu
                    , list.ElementAt(i).taskInfo.average_ram
                    , list.ElementAt(i).taskInfo.average_args
                    );
            }

        }

       
    }
}
