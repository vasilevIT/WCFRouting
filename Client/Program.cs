using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library;

namespace Client
{
    class Program
    {
        private static System.Configuration.Configuration config =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        static void Main(string[] args)
        {
            ThreadPool.SetMaxThreads(Thread.CurrentThread.ManagedThreadId, 20);
            ThreadPool.SetMinThreads(Thread.CurrentThread.ManagedThreadId, 5);
            Console.Title = "Client";
            Console.ReadLine();
            Thread th = new Thread(new ThreadStart(delegate
            {
                Random r = new Random();
                for (int i = 0; true; i++)
                {
                    if (r.Next(1, 100) % 3 == 0)
                    {
                        Task.Run(() => CallService("collection", i));
                    }
                    else
                    {
                        Task.Run(() => CallService("sum", i));
                    }

                    // Console.WriteLine("Продолжить?");
                    // str = Console.ReadLine();
                    // if (str.Equals("n"))
                    // {

                    //    break;
                    //}

                    if (i > 10)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(2000));
                    }
                    else
                    {

                        Thread.Sleep(TimeSpan.FromMilliseconds(2000));
                    }


                }
            }));
            th.Start();
            Console.ReadLine();
            th.Abort();
            Console.WriteLine("Закончили посылать запросы");
            

            Console.ReadKey();


       
        }

        public static Task<String> CallServiceAsync2(IInterface2 proxy)
        {
            return Task.Run(() => proxy.getHostName());
        }
        //public static Task<String> CallServiceAsync(IInterface2 proxy)
        //{
         //   return Task.Run(() => CallService(proxy));
        //}

        public static void CallService(string method, int i)
        {

            Console.WriteLine("CallService #{0}", i);
            Uri address = new Uri(config.AppSettings.Settings["router"].Value);
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);
            ChannelFactory<IInterface2> factory = new ChannelFactory<IInterface2>(binding, endpoint);
            IInterface2 proxy = factory.CreateChannel();

            Random r = new Random();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                String str = "";
                Guid id = Guid.NewGuid();
                if (method == "sum")
                {
                    long x = 450 + r.Next(30);
                    Logger.Log(id, "client", "Client call method LongSum(" + x + ")",
                        new PerfomanceData(new Uri("http://localhost/client1")), 0, 1);
                    str = proxy.LongSum(id, x).ToString();
                    Logger.Log(id, "client", "Client call method LongSum(" + x + ")",
                        new PerfomanceData(new Uri("http://localhost/client1")), 0, 0);
                }
                else
                {
                    long x = 580 + r.Next(30);
                    Logger.Log(id, "client", "Client call method createBigCollection(" + x + ")",
                        new PerfomanceData(new Uri("http://localhost/client1")), 1, 1);

                    str = proxy.createBigCollection(id, Convert.ToInt32(x));

                    Logger.Log(id, "client", "Client call method createBigCollection(" + x + ")",
                        new PerfomanceData(new Uri("http://localhost/client1")), 1, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Не дождались ответа. {0}", e.Message);
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("Задача №"+i+", Время выполнения " + elapsedTime);
        }
    }
}
