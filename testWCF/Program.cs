using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Library;

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
            Console.ReadKey();

            host.Close();

        }
    }
}
