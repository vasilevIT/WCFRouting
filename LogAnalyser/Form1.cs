using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Library;

namespace LogAnalyser
{
    public partial class Form1 : Form
    {
        private List<LogData> list = new List<LogData>();
        private int numberTest = 3;
        public Form1()
        {
            InitializeComponent();
        }

        private void loadData(string dir_name)
        {
            list.Clear();
            int k = 1;
            DirectoryInfo dir = new DirectoryInfo(dir_name);
            if (!dir.Exists)
            {
                MessageBox.Show("Ошибка. Директория " + dir_name + " не существует.");
                return;
            }
            FileInfo[] fis = dir.GetFiles();
            foreach (var fileInfo in fis)
            {
                string path_to_log = fileInfo.FullName;
                if (!File.Exists(path_to_log))
                {
                    break;
                }
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(LogData));
                int counter = 0;
                string line;
                System.IO.StreamReader file =
                    new System.IO.StreamReader(path_to_log);
                while ((line = file.ReadLine()) != null)
                {
                    MemoryStream stream1 = new MemoryStream(Encoding.Unicode.GetBytes(line));
                    var obj = (LogData)ser.ReadObject(stream1);
                    list.Add(obj);
                    counter++;
                }

                file.Close();
                k++;
            }

            int hour = 0;
            for (int i = 0; i < list.Count; i++)
            {

                if (hour == 0)
                {
                    hour = list[i].time.Hour;
                }
                if (hour < list[i].time.Hour)
                {
                    list[i].time = list[i].time.AddHours(-1);
                }

            }
            list.Sort(delegate (LogData x, LogData y)
            {
                if (x.time == null && y.time == null) return 0;
                else if (x.time == null) return -1;
                else if (y.time == null) return 1;
                else return x.time.CompareTo(y.time);
            });
        }
        private void button1_Click(object sender, EventArgs e)
        {
            loadData("log"+ numberTest + "\\Pareto");
            button4_Click(sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            loadData("log"+ numberTest + "\\Pragm");
            button4_Click(sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {

            loadData("log"+ numberTest + "\\RoundRobin");
            button4_Click(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (list.Count < 1)
            {
                return;
            }
            //выводит список задач и время их выполнения
            Dictionary<Guid,string[]> dictionary = new Dictionary<Guid, string[]>();
            for (int i = 0; i < this.list.Count; i++)
            {
                //вычислям время выполнения задачи
                if (!dictionary.ContainsKey(list[i].operation_id))
                {
                    //клиент запустил
                    LogData start = this.list.Find(x => ((x.host == "client")
                    && (x.operation_id == list[i].operation_id)
                    && (x.operation == 1)));

                    //клиент получил ответ
                    LogData end = this.list.Find(x => ((x.host == "client")
                    && (x.operation_id == list[i].operation_id)
                    && (x.operation == 0)));
                    if ((start!=null) && (end != null))
                    {
                        TimeSpan duration = end.time - start.time;
                        dictionary.Add(list[i].operation_id, new string[] { duration.TotalSeconds.ToString(), list[i].task_type.ToString() });
                    }
                }
            }
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();


            this.dataGridView1.Columns.Add("i", "i");
            this.dataGridView1.Columns.Add("guid", "guid");
            this.dataGridView1.Columns.Add("seconds", "seconds");
            this.dataGridView1.Columns.Add("type", "type");
            this.dataGridView1.Rows.Add(dictionary.Count);
            int j = 0,count_cpu=0,count_ram=0;
            double average_time_cpu = 0.0, average_time_ram = 0.0;
            foreach (var value in dictionary)
            {
                dataGridView1[0, j].Value = j;
                dataGridView1[1, j].Value = value.Key;
                dataGridView1[2, j].Value = value.Value[0];
                dataGridView1[3, j].Value = value.Value[1];
                if (Convert.ToInt16(value.Value[1]) == 0)
                {
                    count_cpu++;
                    average_time_cpu += Convert.ToDouble(value.Value[0]);
                }
                else
                {
                    count_ram++;
                    average_time_ram += Convert.ToDouble(value.Value[0]);
                }
                j++;
            }
            info.Text = String.Format("Количество задач\n" +
                                      "Всего: {0}\n" +
                                      "CPU: {1}\n" +
                                      "RAM: {2}\n\n" +
                                      "Среднее время:\n" +
                                      "CPU: {3:F}\n" +
                                      "RAM: {4:F}\n", dictionary.Count, count_cpu,count_ram
                                      ,average_time_cpu/count_cpu
                                      ,average_time_ram/count_ram);

            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (list.Count < 1)
            {
                return;
            }
            //выводит список задач и время их выполнения
            Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
            for (int i = 0; i < this.list.Count; i++)
            {
                //вычислям время выполнения задачи
                if(list[i].host == "server")
                if (!dictionary.ContainsKey(list[i].uri))
                {
                    List<LogData> task1 = list.FindAll(x => (
                        (x.uri == list[i].uri)
                        && (x.host == "server")
                        && (x.operation == 1)
                        && (x.task_type == 0)
                        ));
                    List<LogData> task2 = list.FindAll(x => (
                        (x.uri == list[i].uri)
                        && (x.host == "server")
                        && (x.operation == 1)
                        && (x.task_type == 1)
                        ));
                    //обработанное количество задач каждого типа
                    int count_task1 = task1.Count;
                    int count_task2 = task2.Count;
                    //среднее время обработки каждого типа задач
                    //средняя нагрузка по  CPU && RAM

                    double ram1 = 0.0, ram2 = 0.0, cpu1 = 0.0, cpu2 = 0.0, time1 = 0.0, time2 = 0.0;
                    for (int j = 0; j < task1.Count; j++)
                    {
                        cpu1 += task1[j].cpu;
                        ram1 += task1[j].ram;
                        LogData ld = list.Find(x => ((x.operation_id == task1[j].operation_id) && (x.operation == 0) && (x.uri == task1[j].uri)));
                        TimeSpan duration = task1[j].time- ld.time;
                        time1 += duration.TotalSeconds;
                    }
                    for (int j = 0; j < task2.Count; j++)
                    {
                        cpu2 += task2[j].cpu;
                        ram2 += task2[j].ram;
                        LogData ld = list.Find(x => ((x.operation_id == task2[j].operation_id) && (x.operation == 0) && (x.uri == task2[j].uri)));
                        TimeSpan duration = task2[j].time - ld.time;
                        time2 += duration.TotalSeconds;
                    }

                    dictionary.Add(list[i].uri,new string[] {
                        count_task1.ToString()
                        ,count_task2.ToString()
                        ,(cpu1/task1.Count).ToString("F")
                        ,(ram1/task1.Count).ToString("F")
                        ,(cpu2/task2.Count).ToString("F")
                        ,(ram2/task2.Count).ToString("F")
                        ,(time1/task1.Count).ToString("F")
                        ,(time2/task2.Count).ToString("F")
                    });
                }

            }

            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();


            this.dataGridView1.Columns.Add("i", "i");
            this.dataGridView1.Columns.Add("uri", "uri");
            this.dataGridView1.Columns.Add("count1", "count1");
            this.dataGridView1.Columns.Add("count2", "count2");
            this.dataGridView1.Columns.Add("cpu1", "cpu1");
            this.dataGridView1.Columns.Add("ram1", "ram1");
            this.dataGridView1.Columns.Add("cpu2", "cpu2");
            this.dataGridView1.Columns.Add("ram2", "ram2");
            this.dataGridView1.Columns.Add("time1", "time1");
            this.dataGridView1.Columns.Add("time2", "time2");
            this.dataGridView1.Rows.Add(dictionary.Count);
            int k = 0;
            foreach (var value in dictionary)
            {
                dataGridView1[0, k].Value = k;
                dataGridView1[1, k].Value = value.Key;
                dataGridView1[2, k].Value = value.Value[0];
                dataGridView1[3, k].Value = value.Value[1];
                dataGridView1[4, k].Value = value.Value[2];
                dataGridView1[5, k].Value = value.Value[3];
                dataGridView1[6, k].Value = value.Value[4];
                dataGridView1[7, k].Value = value.Value[5];
                dataGridView1[8, k].Value = value.Value[6];
                dataGridView1[9, k].Value = value.Value[7];
                k++;
            }


        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (list.Count < 1)
            {
                return;
            }

            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Add("i", "i");
            this.dataGridView1.Columns.Add("Time", "Time");
            this.dataGridView1.Columns.Add("host", "host");
            this.dataGridView1.Columns.Add("message_id", "message_id");
            this.dataGridView1.Columns.Add("operation", "operation");
            this.dataGridView1.Columns.Add("uri", "uri");
            this.dataGridView1.Columns.Add("routing_to", "routing_to");

            this.dataGridView1.Rows.Add(list.Count);

            // this.dataGridView1[column, row].Value = "test";
            for (int i = 0; i < list.Count; i++)
            {
                list[i].time = list[i].time.ToUniversalTime();
                this.dataGridView1[0, i].Value = i;

                this.dataGridView1[1, i].Value = String.Format("{0:00}:{1:00}:{2:00}:{3:00}"
                    , list[i].time.Hour
                    , list[i].time.Minute
                    , list[i].time.Second
                    , list[i].time.Millisecond);
                this.dataGridView1[2, i].Value = list[i].host;
                this.dataGridView1[3, i].Value = list[i].operation_id;

                string str = "";
                if (list[i].operation == 0)
                {
                    str = "in";
                }
                else
                {
                    str = "out";
                }
                this.dataGridView1[4, i].Value = str;
                this.dataGridView1[5, i].Value = list[i].uri;
                if (list[i].routing_host != null)
                {
                    this.dataGridView1[6, i].Value = list[i].routing_host.Uri;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
