using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class Logger
    {
        private static string file_name = "log.txt";
        public static void Log(string message,PerfomanceData pd, int task_type, bool isBegin)
        {
            string writing_message = String.Format(
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
            File.AppendAllText(file_name,writing_message);
        }
    }
}
