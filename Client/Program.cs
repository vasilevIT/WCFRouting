using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
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
                String str = Console.ReadLine();
                if (str.Equals("n"))
                {
                    break;
                }
            }

            Console.ReadKey();


       
        }
        public static Task<String> CallServiceAsync(IInterface2 proxy)
        {
            return Task.Run(() => CallService(proxy));
        }

        public static String CallService(IInterface2 proxy)
        {
            Random r = new Random();
            return "LongSum:" + proxy.LongSum(800+r.Next(100)).ToString();
        }
    }
}
