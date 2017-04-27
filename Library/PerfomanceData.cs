using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class PerfomanceData
    {
        private double cpu;
        private double ram;
        private int count_task;
        private IPAddress ip;
        private string type;
        [NonSerialized]
        private PerformanceCounter cpuCounter;
        [NonSerialized]
        private PerformanceCounter ramCounter;

        public PerfomanceData()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            Console.WriteLine("PerfonamceData.PerfomanceData()");
            Console.WriteLine(this.ToString());
        }

        public double Cpu
        {
            get { return cpu; }
            set { cpu = value; }
        }

        public double Ram
        {
            get { return ram; }
            set { ram = value; }
        }

        public int CountTask
        {
            get { return count_task; }
            set { count_task = value; }
        }

        public IPAddress Ip
        {
            get { return ip; }
            set { ip = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public override string ToString()
        {
            return $"cpu: {cpu}, ram: {ram}, count_task: {count_task}, ip: {ip}, type: {type}";
        }


        public void Initilization()
        {
            String strHostName = string.Empty;
            // Getting Ip address of local machine...
            // First get the host name of local machine.
            strHostName = Dns.GetHostName();
            Console.WriteLine("Local Machine's Host Name: " + strHostName);
            // Then using host name, get the IP address list..
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            
            /*
            Console.WriteLine("Нагрузка CPU: {0}%, нагрузка RAM: {1}MB, ip: {2}"
                , cpuCounter.NextValue()
                , ramCounter.NextValue()
                ,"localhost");
            */
            cpu = cpuCounter.NextValue();
            ram = ramCounter.NextValue();
            ip = addr[1];

        }
    }
}
