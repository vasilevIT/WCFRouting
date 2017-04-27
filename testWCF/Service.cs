using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library;

namespace testWCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class Service : IInterface2 , IDisposable , ContractCPU, ContractRAM
    {

        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;

        public Service()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        public string Say(string str)
        {
            Console.WriteLine("Client say: " + str);
            return "OK!";
        }

        public void Check()
        {
            ;
        }

        public string SayHello(string name)
        {
            Console.WriteLine("Client Hello with name: " + name);
            return "Hello " + name;
        }

        public Point Add(Point a, Point b)
        {
            Console.WriteLine("Пользователь вызвал метод Add(Point a[{0},{1}], Point b[{2},{3}]);",a.x,a.y,b.x,b.y);
           // float x1 = cpuCounter.NextValue();
            float x2 = cpuCounter.NextValue();
            Console.WriteLine("Нагрузка CPU: {0}%, нагрузка RAM: {1}MB",x2,ramCounter.NextValue());
            return new Point(a.x + b.x , a.y + b.y);
        }

        public long LongSum(long N)
        {
            float x1 = cpuCounter.NextValue();
            long sum = 0;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        sum = sum + i*(j ^ i) + k;
                    }
                }
            }
            float x2 = cpuCounter.NextValue();
            Console.WriteLine("Нагрузка CPU: {0}%, нагрузка RAM: {1}MB", x2, ramCounter.NextValue());
            return sum;
        }

        public double LongDiv(long N)
        {
            float x1 = cpuCounter.NextValue();

            Double result = Double.MaxValue;
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    for (int k = 0; k < N; k++)
                    {
                        result = result / ((k - (j - i)) + 1);
                    }
                }
            }
            float x2 = cpuCounter.NextValue();
            Console.WriteLine("Нагрузка CPU: {0}%, нагрузка RAM: {1}MB", x2, ramCounter.NextValue());
            return result;
        }

        public void createBigCollection(int N)
        {
            float x1 = cpuCounter.NextValue();
            List<double> list_double = new List<double>();
            List<String> list_string = new List<String>();
            Random r = new Random();
            for (int i = 0; i < N; i++)
            {
                list_double.Add(r.NextDouble());
                list_string.Add(r.NextDouble().GetHashCode().ToString());
            }
            Thread.Sleep(TimeSpan.FromSeconds(5));//ждем, чтобы объекты немного пожили в памяти
            float x2 = cpuCounter.NextValue();
            Console.WriteLine("Нагрузка CPU: {0}%, нагрузка RAM: {1}MB", x2, ramCounter.NextValue());

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
