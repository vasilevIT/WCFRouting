using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Library;
using Router;

namespace testWCF
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
          /*  Uri address = new Uri("http://127.0.0.1:4000/A");
            BasicHttpBinding binding = new BasicHttpBinding();
            Type contract = typeof(IInterface2);
*/
            ServiceHost host = new ServiceHost(typeof(Service));
//            host.AddServiceEndpoint(contract, binding, address);
            host.Open();

            Console.WriteLine("Сервер запущен. Нажмите любую клавишу для закрытия.");
            Console.ReadLine();
            SendCurrentState();
            Console.ReadKey();
 
            host.Close();

        }

        private static void SendCurrentState()
        {
            Uri address = new Uri("http://127.0.0.1:4000/RouterStaff");
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);


            ChannelFactory<IRouterService> factory = new ChannelFactory<IRouterService>(binding, endpoint);
            IRouterService proxy = factory.CreateChannel();
            proxy.SendData(5, 999, 1);
        }
    }
}
