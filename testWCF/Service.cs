using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library;
using Router;

namespace testWCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,ConcurrencyMode = ConcurrencyMode.Multiple)]
    class Service : IInterface2 , IDisposable , ContractCPU, ContractRAM
    {

        PerformanceCounter cpuCounter;
        PerformanceCounter ramCounter;

        public Service()
        {
            Console.WriteLine("Service()");
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
            Console.WriteLine("Check();");
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
            Console.WriteLine("LongSum();");
            long sum = 0;
            try
            {
                Guid task_guid = Guid.NewGuid();
                setTaskGuid(0, Program.nt.getPerfomance().Uri, task_guid);
                Program.nt.getPerfomance().UpdateAvg(0);
                Program.nt.getPerfomance().UpdateArgs(Convert.ToInt32(N), 0);

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                float x1 = cpuCounter.NextValue();
                float y1 = ramCounter.NextValue();
                for (int i = 0; i < N; i++)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(5));
                    for (int j = 0; j < N; j++)
                    {
                        for (int k = 0; k < N; k++)
                        {

                            sum = sum + i*(j ^ i) + k;
                        }
                    }
                }
                float x2 = cpuCounter.NextValue() - x1;
                float y2 = y1 - ramCounter.NextValue();
                stopWatch.Stop();
                Program.nt.getPerfomance().UpdateCpu(x2, 0);
                Program.nt.getPerfomance().UpdateRam(y2, 0);
                Program.nt.getPerfomance().UpdateTimeStamp(stopWatch.Elapsed, 0);
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds/10);

                completeTask(task_guid);
                Program.nt.getPerfomance().CountTask--;
                Console.WriteLine("Нагрузка CPU: {0}%, нагрузка RAM: {1}MB, time: {2}", x2, ramCounter.NextValue(),
                    elapsedTime);
                // Program.nt.getPerfomance().CountTask--;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
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
                        result = (result / (2))*1.5;
                    }
                }
            }
            float x2 = cpuCounter.NextValue();
            Console.WriteLine("Нагрузка CPU: {0}%, нагрузка RAM: {1}MB", x2, ramCounter.NextValue());
            Program.nt.getPerfomance().CountTask--;
           // completeTask();
            return result;
        }

        public string createBigCollection(int N)
        {
            Guid task_guid = Guid.NewGuid();
            setTaskGuid(0, Program.nt.getPerfomance().Uri, task_guid);
            Console.WriteLine("createBigCollection();");
            Program.nt.getPerfomance().UpdateAvg(1);
            Program.nt.getPerfomance().UpdateArgs(Convert.ToInt32(N), 1);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            float x1 = cpuCounter.NextValue();
            float y1 = ramCounter.NextValue();
            List<double> list_double = new List<double>();
            List<String> list_string = new List<String>();
            Random r = new Random();
            String hash = this.GetHashCode().ToString();
            for (int i = 0; i < 5; i++)
            {

                hash += hash;
            }
            for (int i = 0; i < N; i++)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(20));
                for (int j = 0; j < N; j++)
                {
                list_double.Add(r.NextDouble());
                list_string.Add(r.NextDouble().GetHashCode().ToString()
                    +hash
                    );
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(2));//ждем, чтобы объекты немного пожили в памяти

            float x2 = cpuCounter.NextValue() - x1;
            float y2 = y1-ramCounter.NextValue();
            stopWatch.Stop();
            Program.nt.getPerfomance().UpdateCpu(x2, 1);
            Program.nt.getPerfomance().UpdateRam(y2, 1);
            Program.nt.getPerfomance().UpdateTimeStamp(stopWatch.Elapsed, 1);
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            string result = String.Format("Нагрузка CPU: {0}%, нагрузка RAM: {1}MB\nTime: {2}", x2, ramCounter.NextValue(),elapsedTime);
            Console.WriteLine(result);
            Program.nt.getPerfomance().CountTask--;
            completeTask(task_guid);
            return result;

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

        public string getHostName()
        {
            Console.WriteLine("\n\ngetHostName();=== {1} === this.GetHashCode() = {0}", this.GetHashCode(), Program.nt.getPerfomance().Uri.ToString());
            return Program.nt.getPerfomance().Uri.ToString() + " " + this.GetHashCode();
        }

        private void setTaskGuid(int task_type,Uri server,Guid guid)
        {
            Uri address = new Uri(Program.config.AppSettings.Settings["routerService"].Value);
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);
            ChannelFactory<IRouterService> factory = new ChannelFactory<IRouterService>(binding, endpoint);
            IRouterService proxy = factory.CreateChannel();
            proxy.setGuid(task_type, server, guid);
        }
        private void completeTask(Guid guid)
        {
            Uri address = new Uri(Program.config.AppSettings.Settings["routerService"].Value);
            BasicHttpBinding binding = new BasicHttpBinding();
            EndpointAddress endpoint = new EndpointAddress(address);
            ChannelFactory<IRouterService> factory = new ChannelFactory<IRouterService>(binding, endpoint);
            IRouterService proxy = factory.CreateChannel();
            proxy.setColpleteTask(guid);
        }

        public void Dispose()
        {
            Console.WriteLine("Dispose()");
        }
    }
}
