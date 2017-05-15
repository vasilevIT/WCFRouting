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
            Uri address = new Uri("http://localhost:4000/Router");
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);
           

            ChannelFactory<IInterface2> factory = new ChannelFactory<IInterface2>(binding, endpoint);
            IInterface2 proxy = factory.CreateChannel();
            Console.ReadLine();
            for (int i = 0; i < 100; i++)
            {
                MyMessage ms = new MyMessage("*",1,3,0);
                ms = proxy.Calculate(ms);
                Console.WriteLine("calculate! Operation{0}, arg1= {1}, arg2 = {2}, result ={3}",ms.Operation,ms.N1, ms.N2 ,ms.Result);
                //long n = 50;
                //Console.WriteLine("LongSum = " + proxy.LongSum(n));
                //Console.WriteLine("LongDiv = " + proxy.LongDiv(10));
                //proxy.createBigCollection(50);
                //Console.WriteLine("BigCollection()");
                Console.WriteLine("getHostName() = {0}", proxy.getHostName());
                Console.WriteLine("Продолжить?");
                String str = Console.ReadLine();
                if (str.Equals("n"))
                {
                    break;
                }
            }

            Console.ReadKey();


       
        }
    }
}
