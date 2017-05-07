using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        private PerfomanceData x;
        private EndpointAddress address;
        private Random rand = new Random();

        //для главного маршрутизатора
        public Notification()
        {
            dictionary = new Dictionary<Uri, PerfomanceData>();
            x = null;
            address = null;
            Console.WriteLine("Library.Notification()");
        }

        public Notification(EndpointAddress b, PerfomanceData a)
        {
            dictionary = new Dictionary<Uri, PerfomanceData>();
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
            list.StartListener(ref dictionary);
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
            Dictionary<Uri,PerfomanceData> tempDict = new Dictionary<Uri, PerfomanceData>();
            foreach (KeyValuePair<Uri,PerfomanceData> item in dictionary)
            {
                tempDict.Add(item.Key,(PerfomanceData)item.Value.Clone());
            }
            IEnumerator en = tempDict.Values.GetEnumerator();
            int k = tempDict.Count;
            int n = rand.Next(0,3)-1;
            Console.WriteLine("Random.Nex(0,3) = {0}",n);
            int i = 0;
            while(en.MoveNext())
            {
                if (i == n)
                {
                    PerfomanceData pd = (PerfomanceData)en.Current;
                    return new Uri(pd.Uri.AbsoluteUri);
                }
                i++;
            }
            return null;
        }

        public PerfomanceData getPerfomance()
        {
            return x;
        }
    }
}
