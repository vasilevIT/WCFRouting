using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Library;

namespace Router
{
    class Notification
    {
        private TimeSpan interval = TimeSpan.FromSeconds(2);//sec
        private Dictionary<IPAddress, PerfomanceData> dictionary; 
        private PerfomanceData x;

        public Notification(PerfomanceData a)
        {
            dictionary = new Dictionary<IPAddress, PerfomanceData>();
            x = a;
            Console.WriteLine("Notification.Notification(int a = {0});",x);
        }

        private void Sender()
        {
            while (true)
            {
                x.Initilization();
                Console.WriteLine("Notification.Sender(int x = {0});", x.ToString());
                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
ProtocolType.Udp);
                s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

                IPAddress broadcast = IPAddress.Broadcast;

              //  byte[] sendbuf = Encoding.ASCII.GetBytes("hello any body!");
                IPEndPoint ep = new IPEndPoint(broadcast, 11000);
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, x);
                byte[] msg = stream.ToArray();
                s.SendTo(msg, ep);

                Console.WriteLine("Message sent to the broadcast address");
                Thread.Sleep(interval);
            }
        }

        private void Reciever()
        {
            UDPListener list = new UDPListener();
            list.StartListener(ref dictionary);

            while (true)
            {
                Console.WriteLine("Notification.Reciever();");
                Thread.Sleep(interval);
            }
        }

        public void Run()
        {
            //В новых потоках
            Thread thred_send = new Thread(Sender);
            thred_send.Start();

            Thread thred_reciev = new Thread(Reciever);
            thred_reciev.Start();
        }

        public Dictionary<IPAddress,PerfomanceData> getDictionary()
        {
            return dictionary;
        }
    }
}
