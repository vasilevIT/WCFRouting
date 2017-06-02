using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library
{
    public static class Logger
    {
        private static string file_name = "log.txt";

        public static void Log(string message, PerfomanceData pd, short task_type, short isBegin)
        {
            Log(Guid.Empty,message,pd,task_type,isBegin);
        }

        public static void Log(Guid id,string message,PerfomanceData pd, short task_type, short isBegin)
        {
            string writing_message = "";
            /*
            writing_message = String.Format(
                                                   "{0} ={6}= Server URI: {1}" +
                                                   ", CPU: {2}" +
                                                   ", RAM: {3}" +
                                                   ", Count task: {4}" +
                                                   ", This task type: {5}. " +
                                                   "Log message: {7}"
                                                   ,DateTime.Now
                                                   , pd.Uri
                                                   , pd.Cpu
                                                   ,pd.Ram
                                                   ,pd.CountTask
                                                   , task_type
                                                   , Convert.ToDecimal(isBegin)
                                                   ,message
                                                   );
            */
            LogData logData = new LogData("client",task_type,isBegin,pd,message);
            logData.operation_id = id;
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(LogData));
            ser.WriteObject(stream1, logData);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            writing_message = sr.ReadToEnd();
            while (true)
            {
                try
                {
                    File.AppendAllText(file_name, writing_message + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    continue;
                }
                break;
            }
        }
    }
}
