using System;
using System.Diagnostics;
using System.Management;
using System.ServiceModel;

namespace Library
{
    [Serializable]
    public class PerfomanceData : ICloneable
    {
        private double cpu;
        private double ram;
        private int count_task;
        private Uri uri;
        private TaskInfo[] taskInfo = new TaskInfo[]{new TaskInfo(0), new TaskInfo(1)};//2 типа задач
        public ulong totalRam = 0;

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
        

        public override string ToString()
        {
            return $"cpu: {cpu}, ram: {ram}, count_task: {count_task}, uri: {uri}\n"
                +"average_cpu: "+taskInfo[0].average_cpu+", average_ram: "+taskInfo[0].average_ram+", average_time: "
                +taskInfo[0].average_time
                +"\nPerfomanceIndex: " + this.getPerfomanceIndex(0);
        }


        public void Initilization()
        {
            cpu = cpuCounter.NextValue();
            ram = ramCounter.NextValue();
        }

        public void Initilization(EndpointAddress address)
        {

            ManagementObjectSearcher ramMonitor = 
                new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");

            foreach (ManagementObject objram in ramMonitor.Get())
            {
                this.totalRam = Convert.ToUInt64(objram["TotalVisibleMemorySize"])/1000;
            }
            cpu = cpuCounter.NextValue();
            ram = ramCounter.NextValue();
            uri = address.Uri;

        }

        /*
         * обновляет отметку о среднем времени выполнения задачи 
         * определенного типа 
         * 0 - вычислительная задача
         * 1 - создание коллекции
         * 2 - отправка/скачивание данных
         * 
         * на текущем хосте
         */
        public void UpdateTimeStamp(TimeSpan ts, int type_task)
        {
           this.taskInfo[type_task].average_time =  ts.Seconds;
        }

        public void UpdateArgs(int N, int type_task)
        {
           this.taskInfo[type_task].average_args = N;
        }

        public void UpdateCpu(double cpu, int type_task)
        {
            this.taskInfo[type_task].average_cpu = cpu;
        }

        public void UpdateRam(double ram,int type_task)
        {
            this.taskInfo[type_task].average_ram = ram;
        }

        public void UpdateAvg(int type_task)
        {
            if (type_task >= 0 && type_task
                <= 2)
            {
                //UpdateCpu(type_task);
                //UpdateRam(type_task);
            }
        }

        public double getPerfomanceIndex(int type_task)
        {
            if ((type_task >= 0) && (type_task <= 2) && (totalRam > 0))
            {
                return ((taskInfo[type_task].average_cpu / 100) + (taskInfo[type_task].average_ram / totalRam) + 5/taskInfo[type_task].average_time) /3;
            }
            return 0;
        }

        public double calcPrognoseTime()
        {
            /*
             * Предлагаю сделать биленейную интерполяцию по 3 точкам
             * CPU,RAM,TIME
             * (0,0,3.5); // 3.5 - минимальное возможное время выполнения задачи(можно посчитать вручную на листочке)
             * (average_cpu,average_ram,average_time);
             * (1,1,timeout) // где timeout - максимальное возможное время выполнения(1000 секунд, например), но лучше взять секунд 30
             */
            return 5;//secund
        }

        public object Clone()
        {
            PerfomanceData clon = new PerfomanceData();
            clon.Uri = this.Uri;
            clon.CountTask = this.CountTask;
            clon.Cpu = this.Cpu;
            clon.Ram = this.Ram;
            return (object) clon;
        }

        public double getAverageTime(int i)
        {
           return this.taskInfo[i].average_time;
        }

        public TaskInfo getTaskInfo(int i)
        {
            if (i<taskInfo.Length)
                return taskInfo[i];
            return null;
        }
    }
}
