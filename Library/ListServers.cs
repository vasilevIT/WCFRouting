using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class ListServers
    {
        private List<PerfomanceData> servers;
        private bool Sorted = false;

        public ListServers()
        {
            servers = new List<PerfomanceData>();
        }

        public void Add(PerfomanceData pd)
        {
            int index = servers.FindIndex(x => x.Uri == pd.Uri);
            if (index == -1) //не существует
            {
                servers.Add(pd);
            }
            else
            {
                servers[index] = pd;
            }
            this.Sorted = false;
            this.Sort();
        }

        public void Delete(PerfomanceData pd)
        {
            servers.Remove(pd);
        }

        public PerfomanceData Find(PerfomanceData pd)
        {
            PerfomanceData findServer = null;
            /*
             * code
             */
            findServer = servers.Find(x => x.Uri == pd.Uri);
            return findServer;
        }

        private int CompareServers(PerfomanceData A, PerfomanceData B)
        {
            int ires;

            ires = 0;
            if (A.Cpu < B.Cpu)
                ires = -1;
            if (A.Cpu > B.Cpu)
                ires = 1;
            if (A.Cpu == B.Cpu)
            {
                if (A.Ram < B.Ram)
                    ires = -1;
                if (A.Ram > B.Ram)
                    ires = 1;
            }

            return ires;
        }
        public void Sort()
        {
            /*
             * Сортировка по CPU && RAM
             * ищем парето множество по 2 критериям
             * в нем ищем опорную точку 
             */
            //
           // servers.Sort(CompareServers);

            PrintServers();
            Console.WriteLine("BeforeSort");
            //пока только по CPU
            int[] weights = new int[servers.Count];
            List<PerfomanceData> list_pareto = new List<PerfomanceData>();
            List<PerfomanceData> list_pareto_bad = new List<PerfomanceData>();
            List<PerfomanceData> list_pareto_bad_more = new List<PerfomanceData>();
            for (int i = 0; i < servers.Count; i++)
            {
                weights[i] = 0;
                for (int j = 0; j < servers.Count; j++)
                {
                    //можно добавить весовые коэффициенты для параметра CPU и RAM, т.к. они не равнозначны
                    if ((i!=j) 
                        && (servers[i].Cpu > servers[j].Cpu) 
                        && (servers[i].Ram < servers[j].Ram))
                    {
                        weights[i]++;
                    }
                }
                if (weights[i] == 0)
                {
                    list_pareto.Add(servers[i]);
                }
                else if (weights[i] < servers.Count*0.3)
                {
                    list_pareto_bad.Add(servers[i]);
                }
                else
                {
                    list_pareto_bad_more.Add(servers[i]);
                }
            }

            //объединяем 3 списка в один в следующем порядке
            // pareto -> bad -> bad_more
            list_pareto.AddRange(list_pareto_bad);
            list_pareto.AddRange(list_pareto_bad_more);
            this.servers = list_pareto;
            this.Sorted = true;
            PrintServers();
            Console.WriteLine("AfterSort");
        }

        private void PrintServers()
        {
            Console.WriteLine("\n==========PrintServers()==========");
            for (int i = 0; i < servers.Count; i++)
            {
                Console.WriteLine("Server {0}: {1}",i,servers[i].ToString());
            }
            Console.WriteLine("==========PrintServers()==========");
        }

        ~ListServers()
        {
            servers.Clear();
        }

        public Uri GetOptimizeHost()
        {
            if (!this.Sorted)
            {
                this.Sort();
            }
            return this.servers.First().Uri;
        }
    }
}
