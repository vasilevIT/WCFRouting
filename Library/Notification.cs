using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading;

namespace Library
{
    public class Notification
    {
        private TimeSpan interval = TimeSpan.FromSeconds(2);//sec
        private Dictionary<Uri, PerfomanceData> dictionary;
        private ListServers servers;
        private PerfomanceData x;
        private EndpointAddress address;
        private Random rand = new Random();

        //для главного маршрутизатора
        public Notification()
        {
            dictionary = new Dictionary<Uri, PerfomanceData>();
            servers = new ListServers();
            x = null;
            address = null;
            Console.WriteLine("Library.Notification()");
        }

        public Notification(EndpointAddress b, PerfomanceData a)
        {
            dictionary = new Dictionary<Uri, PerfomanceData>();
            servers = new ListServers();
            x = a;
            address = b;
            Console.WriteLine("Library.Notification(int a = {0});", x);
        }

        private void Sender()
        {
            while (true)
            {
                x.Initilization(address);
              //  Console.WriteLine("Library.Sender(int x = {0});", x.ToString());
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
ProtocolType.Udp);
                s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                IPAddress broadcast = IPAddress.Broadcast;
                IPEndPoint ep = new IPEndPoint(broadcast, 11000);
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, x);
                byte[] msg = stream.ToArray();
                s.SendTo(msg, ep);

               // Console.WriteLine("Message sent to the broadcast address");
                Thread.Sleep(interval);
            }
        }

        private void Reciever()
        {
            UDPListener list = new UDPListener();
            list.StartListener(ref servers);
        }

        public void Run()
        {
            //В новых потоках
            if (x != null)
            {
                Thread thred_send = new Thread(Sender);
                thred_send.Start();
            }

            Thread thred_reciev = new Thread(Reciever);
            thred_reciev.Start();
        }

        public Dictionary<Uri,PerfomanceData> getDictionary()
        {
            return dictionary;
        }

        public PerfomanceData getOptimizeHost()
        {
            return servers.GetOptimizeHost();
            // Dictionary<Uri,PerfomanceData> tempDict = new Dictionary<Uri, PerfomanceData>();

            // int n = rand.Next(0, 2);
            //// Console.WriteLine("Random.Nex(0,{0}) = {1}", 2, n);
            // int i = 0;
            // //копирование, чтобы не допустить изменения словаря во время обновления данных

            // //сделать как-нибудь по другому, чтобы коллекция не могла измениться при копировании...
            // Dictionary<Uri, PerfomanceData>.Enumerator enumerator = dictionary.GetEnumerator();
            // Dictionary<Uri, PerfomanceData>.KeyCollection keys = dictionary.Keys;
            // keys.ElementAt(0);
            // for (int k =0;i<keys.Count;k++)
            // {
            //     PerfomanceData item = dictionary[keys.ElementAt(k)];
            //     tempDict.Add(item.Uri, (PerfomanceData)item.Clone());
            //     i++;
            // }

            // //реализация выбора хоста(пока рандом)
            // PerfomanceData pd = null;
            // IEnumerator en = tempDict.Values.GetEnumerator();
            // double min_cpu = 111;
            // Uri min_uri = null;
            // while(en.MoveNext())
            // {
            //     pd = (PerfomanceData)en.Current;
            //     if (min_cpu > pd.Cpu )
            //     {
            //         min_cpu = pd.Cpu;
            //         min_uri = pd.Uri;
            //     }
            // }
            // Console.WriteLine("geteOptimizeHost(); == {0}", tempDict[min_uri].Uri.AbsoluteUri);
            // return new Uri(tempDict[min_uri].Uri.AbsoluteUri);
        }

        public PerfomanceData getPerfomance()
        {
            return x;
        }

        public PerfomanceData getOptimizeHostNoSelf()
        {
            return servers.GetOptimizeHostNoSelf(this.x);
            //try
            //{
            //    Dictionary<Uri, PerfomanceData> tempDict = new Dictionary<Uri, PerfomanceData>();

            //    int n = rand.Next(0, 2);
            //    int i = 0;
            //    //копирование, чтобы не допустить изменения словаря во время обновления данных
            //    foreach (KeyValuePair<Uri, PerfomanceData> item in dictionary)
            //    {
            //        if (item.Key != address.Uri)
            //        {
            //            tempDict.Add(item.Key, (PerfomanceData) item.Value.Clone());
            //        }
            //        i++;
            //    }

            //    //реализация выбора хоста(пока рандом)
            //    PerfomanceData pd = null;
            //    IEnumerator en = tempDict.Values.GetEnumerator();
            //    double min_cpu = 111;
            //    Uri min_uri = null;
            //    i = 0;
            //    while (en.MoveNext())
            //    {
            //        pd = (PerfomanceData) en.Current;
            //        Console.WriteLine(pd.ToString());
            //        if (min_cpu > pd.Cpu)
            //        {
            //            min_cpu = pd.Cpu;
            //            min_uri = pd.Uri;
            //        }
            //        i++;
            //    }

            //    Console.WriteLine("geteOptimizeHostNoSelf(); == {0}", tempDict[min_uri].Uri.AbsoluteUri);
            //    return new Uri(tempDict[min_uri].Uri.AbsoluteUri);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error in getOptimizeHostNoSelf(): " + e.Message);
            //    return null;
            //}
        }

        public PerfomanceData calcServerIndexes(ConcurrentBag<CustomTask> tasks, int taskType)
        {
            Console.WriteLine("calcServerIndexes()");
           List<PerfomanceData> list = servers.getList();
            int optimize_server = 0;
            double optimize_index = 0; // ->max
            for (int i = 0; i < list.Count; i++)
            {
                //получаем список задач сервера
                List<CustomTask> task_info = tasks.ToList().FindAll(x => x.working_server == list[i].Uri);
               
                double index = getServerIndex(list[i],task_info, taskType);
                Console.WriteLine("server {2}: {0} index: {1} tasks: {3}", list[i].Uri, index, i, task_info.Count);
                if (index > optimize_index)
                {
                    optimize_index = index;
                    optimize_server = i;
                }

            }
            Console.WriteLine("Optimize server: {0} index: {1}",list[optimize_server].Uri,optimize_index);
            return list[optimize_server];

        }
        //Считаем свободную площадь прямоугольника ресурсов на ближайшие 2 секунды
        //taskType - тип ресурсов...0-CPU, 1-RAM
        public double getServerIndex(PerfomanceData pd,List<CustomTask> list, int taskType)
        {
            int sec = 5;
            double index = 1.0;//значит нет задач
            double totalResource = 100;//CPU
            double S = sec * totalResource, X = 0.0; // 100 - 100% CPU/RAM
            DateTime now = DateTime.Now;
            DateTime end = DateTime.Now.AddSeconds(sec);
            DateTime indexTime = DateTime.Now;
            list.Sort();
            for (int i = 0; i < list.Count; i++)
            {
                TaskInfo ti = list[i].taskInfo;
                if (ti.average_time > 0)
                {
                    //максимальная длина
                    TimeSpan max_time = end.Subtract(indexTime);
                    //определяем через сколько будет конец...
                    TimeSpan end_time = list[i].start.AddSeconds(ti.average_time).Subtract(indexTime);
                    if (end_time > max_time)
                    {
                        end_time = max_time;
                    }
                    if (end_time.TotalSeconds <= 0)
                    {
                        continue;
                    }
                    if (taskType == 0)
                    {
                        X += ti.average_cpu*(end_time.TotalSeconds); //totalsec до конца отрезка длиной в sec секунды
                    }
                    else
                    {
                        X += (Math.Abs(ti.average_ram)/(pd.totalRam)) * (end_time.TotalSeconds); //totalsec до конца отрезка длиной в sec секунды
                    }
                    //Console.WriteLine("task #{0}: now: {1}, avg_time: {2}, end: {3}, X: {4}", i, now, ti.average_time,
                    //    (end_time.TotalSeconds), X);
                }
            }
            if (X > 0)
            {
                index = (S - X) / S;
            }

            return index;
        }
    }
}
