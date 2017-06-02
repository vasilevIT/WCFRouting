using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;

namespace testWCF
{
    public partial class FormInfo : Form
    {
        public FormInfo()
        {
            InitializeComponent();

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        delegate void SetDataCallback(PerfomanceData pd);
        public void updateField(PerfomanceData pd)
        {
            ;//Task.Run(() => updateFieldUnSave(pd));

        }

        private void updateFieldUnSave(PerfomanceData pd)
        {
            /*
            if (this.countTask1.InvokeRequired)
            {
                SetDataCallback d = new SetDataCallback(updateFieldUnSave);
                this.Invoke(d, new object[] {pd});
            }
            else
            {

                this.countTask1.Text = pd.getTaskInfo(0).count_task.ToString();
            }
            */
            /*
            if (this.countTask2.InvokeRequired)
            {
                SetDataCallback d = new SetDataCallback(updateFieldUnSave);
                this.Invoke(d, new object[] {pd});
            }
            else
            {

                Console.WriteLine("test");
                //this.countTask2.Text = pd.getTaskInfo(1).count_task.ToString();
            }
            */
            /*
            this.countTask2.Text = pd.getTaskInfo(1).count_task.ToString();

            this.time1.Text = pd.getTaskInfo(0).average_time.ToString();
            this.time2.Text = pd.getTaskInfo(1).average_time.ToString();

            this.Cpu.Text = pd.Cpu.ToString();
            this.Ram.Text = pd.Ram.ToString();
            */
        }
    }
}
