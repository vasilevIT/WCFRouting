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
            ThreadPool.SetMaxThreads(Thread.CurrentThread.ManagedThreadId, 10);
            ThreadPool.SetMinThreads(Thread.CurrentThread.ManagedThreadId, 5);
            Console.Title = "Client";
            Console.ReadLine();
            String str;
            for (int i = 0; i < 100; i++)
            {
                // Task<String> task = CallServiceAsync(proxy);

                if (true)
                {
                    Task.Run(() => CallService("sum"));
                }
                else
                {
                    Task.Run(() => CallService("collection"));
                }

                /*
                var awaiter = task.GetAwaiter();
                awaiter.OnCompleted(() => // Продолжение
                {
                    String result = awaiter.GetResult();
                    Console.WriteLine(result); // 116
                }); ;
                */
                Console.WriteLine("Продолжить?");
                str = Console.ReadLine();
                if (str.Equals("n"))
                {
                   break;
               }
              //  factory.Close();
                
                /*

                Task<String> task2 = CallServiceAsync2(proxy);
                var awaiter2 = task2.GetAwaiter();
                awaiter2.OnCompleted(() => // Продолжение
                {
                    String result = awaiter2.GetResult();
                    Console.WriteLine(result); // 116
                }); ;
                Console.WriteLine("Продолжить?");
                str = Console.ReadLine();
                if (str.Equals("n"))
                {
                    break;
                }
                */
            }

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

        public static void CallService(string method)
        {

            Uri address = new Uri(config.AppSettings.Settings["router"].Value);
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);
            ChannelFactory<IInterface2> factory = new ChannelFactory<IInterface2>(binding, endpoint);
            IInterface2 proxy = factory.CreateChannel();
            Console.WriteLine("call async method: {0}", Thread.CurrentThread);
            Random r = new Random();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            String str = "";
            if (method == "sum")
            {
                str = proxy.LongSum(500 + r.Next(100)).ToString();
            }
            else
            {
                str = proxy.createBigCollection(500+r.Next(100));
            }
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
           // Console.WriteLine(proxy.getHostName());
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            Console.WriteLine("LongSum:" + str + "\n" + "Время выполнения " + elapsedTime);
        }
    }
}
