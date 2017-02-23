using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace testWCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class Service : IInterface2 , IDisposable
    {
        public string Say(string str)
        {
            Console.WriteLine("Client say: " + str);
            return "OK!";
        }

        public string SayHello(string name)
        {
            Console.WriteLine("Client Hello with name: " + name);
            return "Hello " + name;
        }

        public Point Add(Point a, Point b)
        {
            Console.WriteLine("Пользователь вызвал метод Add(Point a, Point b);");
            return new Point(a.x + b.x , a.y + b.y);
        }

        public MyMessage Calculate(MyMessage message)
        {
            MyMessage responce = new MyMessage(message);

            switch (responce.Operation)
            {
                case "+":
                    responce.Result = responce.N1 + responce.N2;
                    break;

                case "-":
                    responce.Result = responce.N1 - responce.N2;
                    break;

                case "*":
                    responce.Result = responce.N1 * responce.N2;
                    break;

                case "/":
                    if (responce.N2 == 0)
                    {
                        DivideByZeroException exception = new DivideByZeroException();
                        throw new FaultException<DivideByZeroException>(exception,"Попытка деления на 0.");
                    }
                    responce.Result = responce.N1 / responce.N2;
                    break;

                default:
                    responce.Result = 0.0D;
                    break;
            }
            return responce;

        }

        public void Dispose()
        {
            Console.WriteLine("Dispose()");
        }
    }
}
