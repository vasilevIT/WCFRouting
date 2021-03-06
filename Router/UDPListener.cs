﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Library;

namespace Router
{
    public class UDPListener
    {
            private const int listenPort = 11000;

            public void StartListener(ref ListServers servers)
            {
                bool done = false;
                IPEndPoint localpt = new IPEndPoint(IPAddress.Any, listenPort);

                UdpClient listener = new UdpClient();
                listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                listener.Client.Bind(localpt);
                IPEndPoint groupEP = new IPEndPoint(IPAddress.Broadcast, listenPort);

                try
                {
                    while (!done)
                    {
                       // Console.WriteLine("Waiting for broadcast");
                        byte[] bytes = listener.Receive(ref groupEP);
                        MemoryStream stream = new MemoryStream();
                        BinaryFormatter formatter = new BinaryFormatter();
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Seek(0, SeekOrigin.Begin);
                        PerfomanceData x = (PerfomanceData)formatter.Deserialize(stream);
                       /* Console.WriteLine("Received broadcast from {0} :\n {1}\n",
                        groupEP.ToString(),
                        x.ToString());
                        */
                        servers.Add(x);
                    /*
                    int i = 0;
                    foreach (var item in dictionary)
                    {
                        Console.WriteLine("item{0} = {1}",i,item.ToString());
                        i++;
                    }
                    Console.WriteLine();
                    */
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
