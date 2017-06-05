using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class LogData
    {
        public Guid operation_id = Guid.Empty; 
        public DateTime time;
        public string host = "";
        public short task_type;
        public short operation;
        public string uri;
        public double cpu;
        public double ram;
        public int countTask;
        public string message;
        public PerfomanceData routing_host;

        public LogData(string host, short type, short in_out, PerfomanceData pd,string message)
        {
            time = DateTime.Now;
            this.host = host;
            task_type = type;
            operation = in_out;
            this.message = message;
            if (pd != null)
            {
                uri = pd.Uri.ToString();
                cpu = pd.Cpu;
                ram = pd.Ram;
                countTask = pd.CountTask;
            }
            else
            {
                uri = host;
                cpu = 0;
                ram = 0;
                countTask = 0;
            }

        }
    }
}
