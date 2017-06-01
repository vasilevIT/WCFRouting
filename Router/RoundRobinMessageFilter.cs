//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Threading;
using System.Xml;
using Library;


//this class demonstrates a custom MessageFilter, the Round Robin Message Filter.
//note that this class is provided as an example of how to implement a custom message
//filter.  The Round Robin algorithm here is (probably) not thread safe, but does show
//an interesting implementation which involves the creation of a RoundRobinMessageFilterTable.
//See http://msdn.microsoft.com/en-us/library/ms599871.aspx and http://msdn.microsoft.com/en-us/library/ms599841.aspx
namespace Router
{
    public class RoundRobinMessageFilter : MessageFilter
    {
        RoundRobinGroup group;
        string groupName;

        //perform round robin organized by endpoints in a particular group
        //filters with the same group name will be round-robin'd between, different
        //group names will operate independently
        public RoundRobinMessageFilter(string groupName)
        {
            Console.WriteLine("RoundRobinMessageFilter.RoundRobinMessageFilter(string {0})",groupName);
            if (string.IsNullOrEmpty(groupName)) { throw new ArgumentNullException("groupName"); }

            this.groupName = groupName;
        }
        

        void SetGroup(RoundRobinGroup group)
        {
            Console.WriteLine("RoundRobinMessageFilter.SetGroup()");
            if (group == null) { throw new ArgumentNullException("group"); }

            this.group = group;
        }

        //this custom message filter doesn't actually rely on the 
        //match methods of the messageFilter object in order to determin
        //when things match.  See inside the custom RoundRobinMessageFilterTable
        public override bool Match(Message message)
        {
            throw new NotSupportedException();
        }

        public override bool Match(MessageBuffer buffer)
        {
            throw new NotSupportedException();
        }

        //Message filters can be factories for their parent message filter table.
        protected override IMessageFilterTable<TFilterData> CreateFilterTable<TFilterData>()
        {
            Console.WriteLine("RoundRobinMessageFilter.IMessageFilterTable()");
            return (IMessageFilterTable<TFilterData>)(new RoundRobinMessageFilterTable<IEnumerable<System.ServiceModel.Description.ServiceEndpoint>>());
        }

        //set up the group that will do the round robining
        class RoundRobinGroup
        {
            string name;
            List<RoundRobinMessageFilter> filters = new List<RoundRobinMessageFilter>();
            IEnumerator<RoundRobinMessageFilter> currentPosition;

            public RoundRobinGroup(string name)
            {
                Console.WriteLine("RoundRobinGroup.RoundRobinGroup(string {0})",name);
                this.name = name;
            }

            //get the next filter that will match.
            public RoundRobinMessageFilter GetNext()
            {
               // Console.WriteLine("RoundRobinGroup.GetNext()");
                this.EnsureEnumerator();
                RoundRobinMessageFilter next = (RoundRobinMessageFilter)this.currentPosition.Current;
                this.AdvanceEnumerator();
                return next;
            }
            //get random filter
            //выбирает случаный фильтр из списка
            public RoundRobinMessageFilter GetRandom()
            {
                //Console.WriteLine("RoundRobinGroup.GetRandom()");
                try
                {
                    int i = 0;
                    /*
                    foreach (RoundRobinMessageFilter item in this.filters)
                    {
                        Console.WriteLine("filter[{0}] = {1}"
                            ,i ,item);
                        i++;
                    }
                    Console.WriteLine("RoundRobinGroup.GetRandom() Before get filter 0 ");
                    */
                    Random rn = new Random();
                    RoundRobinMessageFilter next =
                        (RoundRobinMessageFilter) this.filters.ElementAt(rn.Next(0,this.filters.Count));
                    //Console.WriteLine("RoundRobinGroup.GetRandom() After get filter 0 ");
                    //нужно как-то проверить, доступна ли конечная точка или нет(если нет, то удалить ее из списка и выдать новую)
                    return next;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при выборе хоста: {0}",e.Message);
                    return (RoundRobinMessageFilter) this.filters[0];
                }
            }

            //сюда прописать логику поиска лучшего сервера для данного запроса
            //с наибольшим количество оперативки и свободного процессорного времени
            public RoundRobinMessageFilter GetOptimize()
            {
                //пока случайная реализация
               // Console.WriteLine("RoundRobinGroup.GetOptimize()");
                //получаем список всех хостов с данными о их производительности
                Dictionary<Uri, PerfomanceData> dictionary = Router.Program.nt.getDictionary();
                //вычисляем наиболее оптимальный хост для обработки задачи
                Dictionary<Uri, PerfomanceData>.KeyCollection key = dictionary.Keys;
                //находим ключ самого эффективного хоста
                //IPAddress key = new IPAddress();
                //
                Uri ip = key.ElementAt(0);
                Random rm = new Random();
                RoundRobinMessageFilter next = (RoundRobinMessageFilter)this.filters.ElementAt(rm.Next(this.filters.Count()));
                return next;
            }

            private void EnsureEnumerator()
            {
               // Console.WriteLine("RoundRobinGroup.EnsureEnumerator()");
                if (this.currentPosition == null)
                {
                    this.currentPosition = filters.GetEnumerator();
                    this.currentPosition.MoveNext();
                }
            }

            private void AdvanceEnumerator()
            {
               // Console.WriteLine("RoundRobinGroup.AdvanceEnumerator()");
                if (!this.currentPosition.MoveNext())
                {
                    //Reached the end, clear the enumerator
                    this.currentPosition.Dispose();
                    this.currentPosition = null;
                }
            }

            //when asked for a match, see if the enumerator is pointing at 
            //this filter.  If it is, return that this filter matched (true)
            //otherwise, return false
            public bool Match(RoundRobinMessageFilter filter)
            {
                Console.WriteLine("RoundRobinGroup.Match(RoundRobinMessageFilter filter(groupName={0}))",filter.groupName);

                /*
                 Изменить реализацию
                 * 1. Рандомный выбор сервера(например)
                 */

                //получаем первую позициюю(если енумератор пуст)
                //либо текущую
                Console.WriteLine("получаем первую позициюю(если енумератор пуст)");
                this.EnsureEnumerator();
                RoundRobinMessageFilter currentFilter = (RoundRobinMessageFilter)this.currentPosition.Current;
                bool matched = Object.ReferenceEquals(currentFilter, filter);
                if (matched)
                {
                    //если был последний фильтр, то обнуляем енуменатор
                    Console.WriteLine("если был последний фильтр, то обнуляем енуменатор");
                    this.AdvanceEnumerator();
                }
                return matched;
            }

            //add another filter to the internal message filter table
            internal void AddFilter(RoundRobinMessageFilter filter)
            {
                Console.WriteLine("RoundRobinGroup.AddFilter(RoundRobinMessageFilter filter(groupName={0}))", filter.groupName);
                if (this.currentPosition != null)
                {
                    throw new InvalidOperationException("Cannot add while enumerating");
                }
                this.filters.Add(filter);
                filter.SetGroup(this);
            }

        }

        //the custom message filter table class
        class RoundRobinMessageFilterTable<TFilterData> : IMessageFilterTable<TFilterData>
            where TFilterData :
           // System.Collections.Generic.List<System.Collections.Generic.IEnumerable<System.ServiceModel.Description.ServiceEndpoint>>
            IEnumerable<System.ServiceModel.Description.ServiceEndpoint>

        {
            Dictionary<MessageFilter, TFilterData> filters = new Dictionary<MessageFilter, TFilterData>();
            Dictionary<string, RoundRobinGroup> groups = new Dictionary<string, RoundRobinGroup>();

            public RoundRobinMessageFilterTable()
            {
                Console.WriteLine("RoundRobinMessageFilterTable.RoundRobinMessageFilterTable()");
            }

            public bool GetMatchingFilter(MessageBuffer messageBuffer, out MessageFilter filter)
            {
                throw new NotImplementedException();
            }

            public bool GetMatchingFilter(Message message, out MessageFilter filter)
            {
                throw new NotImplementedException();
            }

            //handle both the message and message buffer calls that can come into a MessageFilterTable
            public bool GetMatchingFilters(MessageBuffer messageBuffer, ICollection<MessageFilter> results)
            {
                Console.WriteLine("RoundRobinMessageFilterTable.GetMatchingFilters(MessageBuffer[{0}], ICollection<MessageFilter>)", messageBuffer.ToString());
                bool foundSome = false;
                foreach (RoundRobinGroup group in this.groups.Values)
                {
                    RoundRobinMessageFilter matchingFilter = group.GetNext();
                    results.Add(matchingFilter);
                    foundSome = true;
                }

                return foundSome;
            }

            public bool GetMatchingFilters(Message message, ICollection<MessageFilter> results)
            {
               // Console.WriteLine("RoundRobinMessageFilterTable.GetMatchingFilters(Message[{0}],  ICollection<MessageFilter>)",message.ToString());
                bool foundSome = false;
                foreach (RoundRobinGroup group in this.groups.Values)
                {
                    RoundRobinMessageFilter matchingFilter = group.GetNext();
                    results.Add(matchingFilter);
                    foundSome = true;
                }

                return foundSome;
            }

            //Get matching value(s) is the method that the Routing Service calls during runtime
            public bool GetMatchingValue(MessageBuffer messageBuffer, out TFilterData value)
            {
                Console.WriteLine("RoundRobinMessageFilterTable.GetMatchingValue(MessageBuffer:{0}, out TFilterData)",messageBuffer.CreateMessage().ToString());
                value = default(TFilterData);
                List<TFilterData> results = new List<TFilterData>();
                bool outcome = this.GetMatchingValues(messageBuffer, results);
                if (results.Count > 1)
                {
                    throw new MultipleFilterMatchesException();
                }
                else if (results.Count == 1)
                {
                    value = results[0];
                }
                return outcome;
            }

            //вызывается при получении сообщения
            public bool GetMatchingValue(Message message, out TFilterData value)
            {
              //  Console.WriteLine("RoundRobinMessageFilterTable.GetMatchingValue(Message[{0}], out TFilterData",message.ToString());
                value = default(TFilterData);
                List<TFilterData> results = new List<TFilterData>();
                //получаем endPoint
                bool outcome = this.GetMatchingValues(message, results);
                if (results.Count > 1)
                {
                    throw new MultipleFilterMatchesException();
                }
                else if (results.Count == 1)
                {
                    value = results[0];

                }
               // System.Collections.Generic.List<System.Collections.Generic.IEnumerable<> endpoint = value;
                //уходит одна serviceendpoin, а сообщение отсылается на другую(которую первый раз указали
                return outcome;
            }

            public bool GetMatchingValues(MessageBuffer messageBuffer, ICollection<TFilterData> results)
            {
                Console.WriteLine("RoundRobinMessageFilterTable.GetMatchingValues(MessageBuffer[{0}], ICollection<TFilterData>)",messageBuffer.CreateMessage().ToString());
                bool foundSome = false;
                foreach (RoundRobinGroup group in this.groups.Values)
                {
                    RoundRobinMessageFilter matchingFilter = group.GetNext();
                    results.Add(this.filters[matchingFilter]);
                    foundSome = true;
                }
                //check results

                return foundSome;
            }

            public bool GetMatchingValues(Message message, ICollection<TFilterData> results)
            {

                Console.WriteLine("TIME:{2} RoundRobinMessageFilterTable.GetMatchingValues(Message[{0}], ICollection<TFilterData> [{1}])"
                    , message.ToString()
                    , results.GetHashCode()
                    , DateTime.Now.ToString());
                Console.WriteLine("Thread.Hashcode(): {0} ", Thread.CurrentThread.GetHashCode() );
                message.Headers.Add(MessageHeader.CreateHeader("TTL", "", 1));

                //определяем тип задачи
                int N = 0,
                task_type = 0;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(message.ToString());
                XmlNamespaceManager manager = new XmlNamespaceManager(xml.NameTable);
                manager.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                XmlNode xn = xml.SelectSingleNode("//s:Body", manager);
                if (xn != null)
                {
                    string firstName = xn.InnerText; // arg N
                    XmlNode method = xn.LastChild;
                    XmlNode childNode = xn.FirstChild; //arg N
                    if (method.Name == "LongSum")
                    {
                        task_type = 0;
                    }
                    else if (method.Name == "createBigCollection")
                    {
                        task_type = 1;
                    }
                    else
                    {
                        task_type = -1;
                    }
                    N = Int32.Parse(childNode.InnerText);
                    Console.WriteLine("method = {0}", method.Name);
                    Console.WriteLine("N = {0}", childNode.InnerText);
                }
                //message.Headers.RemoveAt(0);
                bool foundSome = false;
                foreach (RoundRobinGroup group in this.groups.Values)
                {
                    RoundRobinMessageFilter matchingFilter = null;
                    ServiceEndpoint endpoint = null;
                    while (this.filters.Count > 0)
                    {
                        PerfomanceData host = Program.nt.calcServerIndexes(Program.tasks, task_type); //Program.nt.getOptimizeHost();
                        Uri uri = host.Uri;
                        Console.WriteLine("Optimize Host:{0}",uri.ToString());
                        try
                        {
                            matchingFilter = GetByUri(group, uri);
                            endpoint = this.filters[matchingFilter].ElementAt(0);
                            

                            CustomTask ct = new CustomTask(TimeSpan.FromSeconds(host.getAverageTime(task_type)), task_type, host.Uri);
                            ct.taskInfo = host.getTaskInfo(task_type);
                            double k = 0;
                            if (ct.taskInfo.average_args > 0)
                            {
                               k = ct.taskInfo.average_time/ct.taskInfo.average_args;
                                // коэффициент  роста времени от размера входных данных
                                double add_sec = (N - ct.taskInfo.average_args) * k;
                                double power = 1.0/3.0;
                                if (add_sec < 0)
                                {
                                    add_sec = -Math.Pow(Math.Abs(add_sec), power);
                                }
                                else
                                {
                                    add_sec = Math.Pow(add_sec, power);
                                }
                                ct.taskInfo.addSomeSeconds(add_sec);// увеличили или уменьшили время выполнения задачи

                            }
                            Program.tasks.Add(ct);
                           // List<CustomTask> list = Program.getTasksByServer(uri,task_type);
                           //Program.nt.calcServerIndexes(Program.tasks,task_type);//Program.getServerIndex(list);
                           //Program.printTasks(list);

                            BasicHttpBinding binding = new BasicHttpBinding();
                            ChannelFactory<IInterface> factory = new ChannelFactory<IInterface>(binding,
                                endpoint.Address);
                            IInterface proxy = factory.CreateChannel();
                           //  proxy.Check();
                            
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Конечная точка не  доступна. " + e.Message);
                          //  this.filters.Remove(matchingFilter);//удаляем точку
                            Program.nt.getDictionary().Remove(uri);
                            continue;
                        }
                    }
                    if (this.filters.Count < 1)
                    {
                        throw new Exception("Нет доступных конечных точек.");
                    }
                    //тут добавляем конечную точку сервиса на которую уйдет наше сообщение
                    TFilterData filter = this.filters[matchingFilter];
                    results.Add(filter);
                    foundSome = true;
                }

                return foundSome;
            }


            //выбор оптимального хоста из группы по URI
            public RoundRobinMessageFilter GetByUri(RoundRobinGroup group, Uri uri)
            {

                RoundRobinMessageFilter filter = null;
                int i = 0;
                int max = this.filters.Count;
                while (i < max)
                {
                    filter = group.GetNext();
                    if (this.filters[filter].ElementAt(0).Address.Uri == uri)
                    {
                        break;
                    }
                }
                return filter;//this.filters[filter].ElementAt(0);
            }

            //add a message filter to the MessageFilterTable
            public void Add(MessageFilter key, TFilterData value)
            {
                RoundRobinMessageFilter filter = (RoundRobinMessageFilter)key;
                Console.WriteLine("RoundRobinMessageFilterTable.Add(MessageFilter[{0}], TFilterData[{1}])"
                    , filter.groupName
                    ,value.GetType().ToString());
                RoundRobinGroup group;
                if (!this.groups.TryGetValue(filter.groupName, out group))
                {
                    group = new RoundRobinGroup(filter.groupName);
                    this.groups.Add(filter.groupName, group);
                }
                group.AddFilter(filter);
                this.filters.Add(key, value);
            }


            //these are all safe to not implement because the Routing Service will never call them during runtime.
            public bool ContainsKey(MessageFilter key)
            {
                throw new NotImplementedException();
            }

            public ICollection<MessageFilter> Keys
            {
                get { throw new NotImplementedException(); }
            }

            public bool Remove(MessageFilter key)
            {
                throw new NotImplementedException();
            }

            public bool TryGetValue(MessageFilter key, out TFilterData value)
            {
                throw new NotImplementedException();
            }

            public ICollection<TFilterData> Values
            {
                get { throw new NotImplementedException(); }
            }

            public TFilterData this[MessageFilter key]
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public void Add(KeyValuePair<MessageFilter, TFilterData> item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(KeyValuePair<MessageFilter, TFilterData> item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(KeyValuePair<MessageFilter, TFilterData>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public bool Remove(KeyValuePair<MessageFilter, TFilterData> item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<KeyValuePair<MessageFilter, TFilterData>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
