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
            Uri address = new Uri("http://127.0.0.1:4000/Router");
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);

            ChannelFactory<IInterface2> factory = new ChannelFactory<IInterface2>(binding, endpoint);
            IInterface2 proxy = factory.CreateChannel();
            Console.ReadLine();
            for (int i = 0; i < 100; i++)
            {


                Console.WriteLine("Server say: " + proxy.Say("Hello Server! message#" + i + "!").Trim());
                Console.WriteLine("Server Hello say: " + proxy.SayHello("Anton").Trim());

                Random r = new Random();

                Point A = new Point(r.NextDouble() * 1000, r.NextDouble() * 1000);
                Point B = new Point(r.NextDouble() * 1000, r.NextDouble() * 1000);

                Point res = proxy.Add(A, B);
                Console.WriteLine("Сервер сложил эти точкии и вернул сумму {0}, {1}", res.x, res.y);
                Console.WriteLine("Продолжить?");
                String str = Console.ReadLine();
                if (str.Equals("n"))
                {
                    break;
                }
/*
                Point A = new Point(1, 2);
                Point B = new Point(3, 5.5);
                Console.WriteLine("Создали точку с координатами 1, 2");
                Console.WriteLine("Создали точку с координатами 3, 5.5");

                Point res = proxy.Add(A, B);
                Console.WriteLine("Сервер сложил эти точкии и вернул сумму {0}, {1}", res.x, res.y);

                MyMessage request = new MyMessage();
                request.Operation = "/";
                request.N1 = 10.22;
                request.N2 = 0;
                try
                {
                    MyMessage responce = proxy.Calculate(request);
                    Console.WriteLine("Сервер прочитал сообщение и вернул ответ {0}", responce.Result);
                }
                catch (FaultException ex)
                {
                    Console.WriteLine("Получили исключение: " + ex.GetType().ToString());
                    Console.WriteLine(ex.Message);
                }*/
               // Console.ReadKey();
            }

            Console.ReadKey();


       
        }
    }
}
