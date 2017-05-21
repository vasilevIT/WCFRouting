using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class PerfomanceData : ICloneable
    {
        private double cpu;
        private double ram;
        private int count_task;
        private Uri uri;
        private string type;
        [NonSerialized]
        private PerformanceCounter cpuCounter;
        [NonSerialized]
        private PerformanceCounter ramCounter;

        public PerfomanceData()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            cpuCounter.NextValue();
            ramCounter.NextValue();
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

        public Uri Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public override string ToString()
        {
            return $"cpu: {cpu}, ram: {ram}, count_task: {count_task}, uri: {uri}, type: {type}";
        }


        public void Initilization(EndpointAddress address)
        {
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
           // Console.WriteLine("Local Machine's Host Name: " + strHostName);
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            cpu = cpuCounter.NextValue();
            ram = ramCounter.NextValue();
            uri = address.Uri;

        }

        public object Clone()
        {
            PerfomanceData clon = new PerfomanceData();
            clon.Uri = this.Uri;
            clon.CountTask = this.CountTask;
            clon.Cpu = this.Cpu;
            clon.Ram = this.Ram;
            clon.Type = this.Type;
            return (object) clon;
        }
    }
}
