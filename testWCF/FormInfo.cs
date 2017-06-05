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
        
        public void updateField(PerfomanceData pd)
        {

            this.countTask1.Text = pd.getTaskInfo(0).count_task.ToString();
            this.countTask2.Text = pd.getTaskInfo(1).count_task.ToString();

            this.time1.Text = String.Format("{0:F} sec", pd.getTaskInfo(0).average_time);
            this.time2.Text = String.Format("{0:F} sec", pd.getTaskInfo(1).average_time);

            this.Cpu.Text = String.Format("{0:F}%",pd.Cpu);
            this.Ram.Text = String.Format("{0:F} MB", pd.Ram);

        }
        
        public void AddLog(string message)
        {
            DateTime now = DateTime.Now;
            LogBox.AppendText(String.Format("{0:00}:{1:00}:{2:00}: {3}{4}"
                ,now.Hour
                , now.Minute
                , now.Second
                , message, Environment.NewLine));
            LogBox.SelectionStart = LogBox.Text.Length;
            LogBox.ScrollToCaret();
        }
    }
}
