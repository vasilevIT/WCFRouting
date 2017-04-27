using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class UDPListener
    {
            private const int listenPort = 11000;

            public void StartListener(ref Dictionary<IPAddress,PerfomanceData> dictionary)
            {
                bool done = false;

                UdpClient listener = new UdpClient(listenPort);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, listenPort);

                try
                {
                    while (!done)
                    {
                        Console.WriteLine("Waiting for broadcast");
                        byte[] bytes = listener.Receive(ref groupEP);
                        MemoryStream stream = new MemoryStream();
                        BinaryFormatter formatter = new BinaryFormatter();
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Seek(0, SeekOrigin.Begin);
                        PerfomanceData x = (PerfomanceData)formatter.Deserialize(stream);
                        Console.WriteLine("Received broadcast from {0} :\n {1}\n",
                        groupEP.ToString(),
                        x.ToString());
                        if (dictionary.ContainsKey(x.Ip))
                        {
                            dictionary[x.Ip] = x;
                        }
                        else
                        {
                            dictionary.Add(x.Ip,x);
                        }
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    listener.Close();
                }
            }
        }
}
