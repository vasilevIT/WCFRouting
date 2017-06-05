//----------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//----------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
namespace testWCF
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
                    */
                    //Console.WriteLine("RoundRobinGroup.GetRandom() Before get filter 0 ");
                    RoundRobinMessageFilter next =
                        (RoundRobinMessageFilter) this.filters[0];
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
                Console.WriteLine("RoundRobinGroup.GetOptimize()");
                //получаем список всех хостов с данными о их производительности
                Dictionary<Uri, PerfomanceData> dictionary = Program.nt.getDictionary();
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
                Console.WriteLine("RoundRobinMessageFilterTable.GetMatchingFilters(Message[{0}],  ICollection<MessageFilter>)",message.ToString());
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
               // Console.WriteLine("RoundRobinMessageFilterTable.GetMatchingValues(MessageBuffer[{0}], ICollection<TFilterData>)",messageBuffer.CreateMessage().ToString());
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
                int TTL = 0;
               // Console.WriteLine("Message HashCode1: {0}", message);
                // message = TransformMessage(message);
                try
                {
                    string hi = message.Headers.GetHeader<string>("TTL", "");
                    TTL = Int32.Parse(hi);
                    TTL = TTL - 1;
                    message.Headers.RemoveAt(0);
                    message.Headers.Add(MessageHeader.CreateHeader("TTL", "", TTL));
                }
                catch (Exception e)
                {
                    ;
                }
              //  Console.WriteLine("Message HashCode2: {0}",message);
               // Console.WriteLine("RoundRobinMessageFilterTable.GetMatchingValues(Message[{0}], ICollection<TFilterData> [{1}])",message.ToString(),results.GetType());
                Console.WriteLine("TIME:{2} RoundRobinMessageFilterTable.GetMatchingValues(Message[{0}], ICollection<TFilterData> [{1}])"
                    ,message.GetHashCode()
                    ,results.GetHashCode()
                    ,DateTime.Now.ToString());
                Console.WriteLine("TTL = {0}",TTL);

                //определяем тип задачи
                int N = 0,
                task_type = 0;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(message.ToString());
                XmlNamespaceManager manager = new XmlNamespaceManager(xml.NameTable);
                manager.AddNamespace("s", "http://schemas.xmlsoap.org/soap/envelope/");
                XmlNode xn = xml.SelectSingleNode("//s:Body", manager);
                Guid messageId = Guid.Empty;
                if (xn != null)
                {
                    string firstName = xn.InnerText; // arg N
                    XmlNode method = xn.LastChild;
                    XmlNode childNode = xn.FirstChild.LastChild; //arg N
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
                    string guidStr = xn.FirstChild.FirstChild.InnerText;
                    messageId = new Guid(guidStr);
                }
                
                Console.WriteLine("messageId = {0}", messageId);
                Console.WriteLine("N = {0}", N);
                bool foundSome = false;
                foreach (RoundRobinGroup group in this.groups.Values)
                {
                    RoundRobinMessageFilter matchingFilter = null;
                    ServiceEndpoint endpoint = null;
                    Random r = new Random();
                    while (this.filters.Count > 0)
                    {
                        Program.nt.getPerfomance().Initilization();
                        PerfomanceData optimize_host = null;
                        try
                        {
                            optimize_host = Program.nt.getOptimizeHostNoSelf();
                            //выбираем хост их списка или выполняем сами
                            //если выполнить быстрее, чем пересылать
                            /*
                            if (optimize_host != null)
                            { Console.WriteLine("Optimize Host:{0}", optimize_host.ToString());}
                            else
                            { Console.WriteLine("Optimize Host: ==SELF== {0}", Program.nt.getPerfomance().Uri.ToString()); }
                            */
                            if ((TTL > 0)
                                && (optimize_host != null)
                                && (
                                ((optimize_host.Cpu<Program.nt.getPerfomance().Cpu) && (task_type == 0))
                                    || ((optimize_host.Ram > Program.nt.getPerfomance().Ram) && (task_type == 1))
                                    )
                                )
                            {
                                Uri uri = optimize_host.Uri;
                                if (uri != null)
                                {
                                    Console.WriteLine("Routing to Host:{0}", uri.ToString());
                                    Program.Log(String.Format("Routing to Host:{0}", uri.ToString()));
                                    matchingFilter = GetByUri(group, uri);
                                    endpoint = this.filters[matchingFilter].ElementAt(0);

                                    //проверка доступности
                                    BasicHttpBinding binding = new BasicHttpBinding();
                                    EndpointAddress endpoint_check = new EndpointAddress(endpoint.Address.Uri.ToString().Replace("Router", ""));
                                    ChannelFactory<IInterface> factory = new ChannelFactory<IInterface>(binding,
                                        endpoint_check);
                                    IInterface proxy = factory.CreateChannel();
                                    proxy.Check();

                                    Logger.Log(messageId, "router", "routing"
                                        , Program.nt.getPerfomance(), Convert.ToInt16(task_type), 0,optimize_host);
                                  
                                }
                            }
                            else
                            {
                                Program.nt.getPerfomance().CountTask++;
                                Uri uri_self = Program.nt.getPerfomance().Uri;
                                string str = uri_self.AbsoluteUri.Replace("Router", "");
                                Console.WriteLine("Routing inside host() :  " + str);
                                Program.Log(String.Format("Routing inside host() :  " + str));
                                uri_self = new Uri(str);
                                matchingFilter = GetByUri(group, uri_self);
                                endpoint = this.filters[matchingFilter].ElementAt(0);
                            }
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Конечная точка не  доступна. " + e.Message);
                            Program.Log(String.Format("Конечная точка не  доступна. " + e.Message));
                            Program.nt.getListServers().Delete(optimize_host);
                            Thread.Sleep(TimeSpan.FromMilliseconds(500));
                          //  this.filters.Remove(matchingFilter);//удаляем точку
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
                    filter =group.GetNext();
                    if (this.filters[filter].ElementAt(0).Address.Uri == uri)
                    {
                        return filter;
                    }
                    i++;
                }
                throw new Exception(String.Format("Не найдена такая конечная точка ({0})",uri));//this.filters[filter].ElementAt(0);
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


            private Message TransformMessage(Message oldMessage)
            {

                Console.WriteLine("TransformMessage();\nOldMessage: " + oldMessage.ToString());
               // Message newMessage = null;
                
                //Message tmpMessage = msgbuf.CreateMessage();
                if (oldMessage.Headers.Count > 2)
                {

                    /* MessageBuffer mb = oldMessage.CreateBufferedCopy(Int32.MaxValue);
                     Message tempMessage = mb.CreateMessage();
                     XmlDictionaryReader xdr = tempMessage.Headers.GetReaderAtHeader(1); //.GetReaderAtBodyContents();

                     XmlDocument xdoc = new XmlDocument();
                     xdoc.Load(xdr);
                     xdr.Close();

                     XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
                     nsmgr.AddNamespace("h", "http://tempuri.org/");
                     XmlNode node = xdoc.SelectSingleNode("h:TTL",nsmgr);
                     if (node != null)
                     {
                         node.InnerText = (Int32.Parse(node.InnerText) - 1).ToString();

                         XmlWriterSettings settings = new XmlWriterSettings();
                         settings.ConformanceLevel = ConformanceLevel.Fragment;

                         MemoryStream ms = new MemoryStream();
                         XmlWriter xw = XmlWriter.Create(ms, settings);
                         XmlDocumentFragment xdf = xdoc.CreateDocumentFragment();
                         xdf.WriteTo(xw);//Save(xw);
                        // xw.Flush();
                         //xw.Close();
                        // xw.

                         ms.Position = 0;
                         XmlReader xr = XmlReader.Create(ms);

                         MemoryStream ms2 = new MemoryStream();
                         XmlWriter xw2 = XmlWriter.Create(ms2);
                         XmlDictionaryReader xdr2 = tempMessage.GetReaderAtBodyContents();

                         XmlDocument xdoc2 = new XmlDocument();
                         xdoc2.Load(xdr2);
                         xdr2.Close();
                         xdoc2.Save(xw2);
                         xw2.Flush();
                         xw2.Close();
                         XmlDictionaryWriter xdw = XmlDictionaryWriter.CreateBinaryWriter(ms2);
                         //create new message from modified XML document


                         //BodyWriterMessage - надо конвертировать обратнов message
                         Message newMessage = Message.CreateMessage(oldMessage.Version, null, xdw);
                         //newMessage.Headers.CopyHeaderFrom(tempMessage, 0);
                         //newMessage.Headers.CopyHeaderFrom(tempMessage, 1);
                         //newMessage.Headers.CopyHeaderFrom(tempMessage, 2);
                         newMessage.Headers.CopyHeadersFrom(oldMessage);
                         newMessage.Headers.WriteHeaderContents(1, xw);
                        // newMessage.WriteBody(xdw);
                         newMessage.Properties.CopyProperties(tempMessage.Properties);
                         //newMessage.Properties.Keys.Remove("AllowOutputBatching");
                         //newMessage.Properties.Values.Remove("AllowOutputBatching");

                        // Console.WriteLine("TransformMessage();\nNewMessage: " + newMessage.ToString());
                        */

                    oldMessage.Headers.Add(MessageHeader.CreateHeader("name_new", "", Guid.NewGuid()));
                    return oldMessage;
                  //  }
                }
                return oldMessage;
            }
        }
    }
}
