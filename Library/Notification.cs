using System;
using System.Collections;
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

        public Uri getOptimizeHost()
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

        public Uri getOptimizeHostNoSelf()
        {
            return servers.GetOptimizeHost();
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
    }
}
