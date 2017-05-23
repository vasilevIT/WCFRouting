using System;
using System.Collections.Generic;
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
        static void Main(string[] args)
        {

            Console.Title = "Client";
            Uri address = new Uri("http://192.168.1.42:4000/Router");
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);
            ChannelFactory<IInterface2> factory = new ChannelFactory<IInterface2>(binding, endpoint);
            IInterface2 proxy = factory.CreateChannel();
            Console.ReadLine();
            String str;
            for (int i = 0; i < 100; i++)
            {
                //long n = 50;
                //Console.WriteLine("LongSum = " + proxy.LongSum(n));
                //Console.WriteLine("LongDiv = " + proxy.LongDiv(10));
                //proxy.createBigCollection(50);
                //Console.WriteLine("BigCollection()");
                
                Task<String> task = CallServiceAsync(proxy);
                var awaiter = task.GetAwaiter();
                awaiter.OnCompleted(() => // Продолжение
                {
                    String result = awaiter.GetResult();
                    Console.WriteLine(result); // 116
                }); ;
                Console.WriteLine("Продолжить?");
                str = Console.ReadLine();
                if (str.Equals("n"))
                {
                    break;
                }
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
        public static Task<String> CallServiceAsync(IInterface2 proxy)
        {
            return Task.Run(() => CallService(proxy));
        }

        public static String CallService(IInterface2 proxy)
        {
            Random r = new Random();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            String str = proxy.LongSum(800 + r.Next(100)).ToString();
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
           // Console.WriteLine(proxy.getHostName());
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            return "LongSum:" + str + "\n" + "Время выполнения " + elapsedTime;
        }
    }
}
