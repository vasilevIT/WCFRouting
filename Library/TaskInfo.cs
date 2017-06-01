using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [Serializable]
    public class TaskInfo
    {
        public int type;
        private double cpu = 0;
        private double ram = 0;
        private double time = 0;
        private double arg = 0;
        public double average_cpu
        {
            get { return cpu; }
            set
            {
                if (cpu == 0) cpu = value;
                else cpu = (cpu + value) / 2;
            }
        }
        public double average_ram
        {
            get { return ram; }
            set
            {
                if (ram == 0) ram = value;
                else ram = (ram + value) / 2;
            }
        }
        public double average_time
        {
            get { return time; }
            set
            {
                if (time == 0) time = value;
                else time = (time + value)/2;
            }
        }
        public double average_args
        {
            get { return arg; }
            set
            {
                if (arg == 0) arg = value;
                else arg = (arg + value) / 2;
            }
        }
        public TaskInfo()
        {
            type = 0;
            average_cpu = 0;
            average_ram = 0;
            average_time = 0;
            average_args = 0;
        }

        public TaskInfo(int type)
        {
            this.type = type;
            average_cpu = 0;
            average_ram = 0;
            average_time = 0;
            average_args = 0;
        }

        public void addSomeSeconds(double sec)
        {
            time += sec;
        }
    }
}
