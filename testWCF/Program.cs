using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Routing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

        public static FormInfo formInfo = new FormInfo();

        static void Main(string[] args)
        {
            
            ThreadPool.SetMaxThreads(Thread.CurrentThread.ManagedThreadId, 10);
           // ThreadPool.SetMinThreads(Thread.CurrentThread.ManagedThreadId, 5);
            System.Net.ServicePointManager.DefaultConnectionLimit = 10;

            //Application.Run(new FormInfo());

            //    Console.Title = "Server";
            Thread viewerThread = new Thread(delegate ()
            {
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
                nt.getListServers().host = routing_host;
                nt.Run();
            });

            viewerThread.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
            viewerThread.Start();

            Thread viewerThread2 = new Thread(delegate()
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        if(Program.nt!=null)
                            if(Program.nt.getPerfomance() != null)
                             formInfo.Invoke(new Action(() => formInfo.updateField(Program.nt.getPerfomance())));
                    }
                    catch (Exception e)
                    {
                        Program.Log("Ошибка." + e.Message);
                    }
                }
            });
            viewerThread2.SetApartmentState(ApartmentState.STA); // needs to be STA or throws exception
            viewerThread2.Start();

            Application.Run(formInfo);
            //Console.WriteLine("Данные сервера на момент запуска: {0}",pr.ToString());
            Console.WriteLine("Сервер запущен. Нажмите любую клавишу для закрытия.");
            Console.ReadLine();
          //  Console.ReadKey();
 
           // host.Close();

        }

        public static void Log(string message)
        {
            formInfo.Invoke(new Action(() => formInfo.AddLog(message)));

        }
    }
}
