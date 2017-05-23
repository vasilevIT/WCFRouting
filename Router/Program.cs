﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Routing;
using Library;

namespace Router
{
    public class Program
    {

        public static Notification nt;
        public static bool isRouter = true;

        static void Main(string[] args)
        {
           Console.Title = "Router";

            ServiceHost host = new ServiceHost(typeof(RoutingService));

           try
           {
               host.Open();
               PrintServiceInfo(host);
           }
           catch (Exception ex)
           {
               Console.WriteLine(ex.Message);
               host.Abort();
               Console.ReadKey();
           }
            

            EndpointAddress address = host.Description.Endpoints[0].Address;

            PerfomanceData pr = new PerfomanceData();
            pr.Initilization(address);
            nt = new Notification();
            nt.Run();

            Console.WriteLine("Сервер запущен. Нажмите любую клавишу для закрытия.");
            Console.ReadKey();
            host.Close();
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
