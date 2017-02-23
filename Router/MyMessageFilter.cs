using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Router
{
    public class MyMessageFilter : MessageFilter
    {
        public MyMessageFilter()
        {
        }

        public override bool Match(MessageBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override bool Match(Message message)
        {
            throw new NotImplementedException();
        }
        protected override IMessageFilterTable<TFilterData> CreateFilterTable<TFilterData>()
        {
            return new MyMessageFilterTable<TFilterData>();
        }

        class MyMessageF
        {
            
        }

        class MyMessageFilterTable<TFilterTable> : IMessageFilterTable<TFilterTable>
        {
            public MyMessageFilterTable()
            {
            }

            public IEnumerator<KeyValuePair<MessageFilter, TFilterTable>> GetEnumerator()
            {
                throw new NotImplementedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(KeyValuePair<MessageFilter, TFilterTable> item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(KeyValuePair<MessageFilter, TFilterTable> item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(KeyValuePair<MessageFilter, TFilterTable>[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(KeyValuePair<MessageFilter, TFilterTable> item)
            {
                throw new NotImplementedException();
            }

            public int Count { get; private set; }
            public bool IsReadOnly { get; private set; }
            public bool ContainsKey(MessageFilter key)
            {
                throw new NotImplementedException();
            }

            public void Add(MessageFilter key, TFilterTable value)
            {
                throw new NotImplementedException();
            }

            public bool Remove(MessageFilter key)
            {
                throw new NotImplementedException();
            }

            public bool TryGetValue(MessageFilter key, out TFilterTable value)
            {
                throw new NotImplementedException();
            }

            public TFilterTable this[MessageFilter key]
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }

            public ICollection<MessageFilter> Keys { get; private set; }
            public ICollection<TFilterTable> Values { get; private set; }
            public bool GetMatchingValue(Message message, out TFilterTable value)
            {
                throw new NotImplementedException();
            }

            public bool GetMatchingValue(MessageBuffer messageBuffer, out TFilterTable value)
            {
                throw new NotImplementedException();
            }

            public bool GetMatchingValues(Message message, ICollection<TFilterTable> results)
            {
                throw new NotImplementedException();
            }

            public bool GetMatchingValues(MessageBuffer messageBuffer, ICollection<TFilterTable> results)
            {
                throw new NotImplementedException();
            }

            public bool GetMatchingFilter(Message message, out MessageFilter filter)
            {
                throw new NotImplementedException();
            }

            public bool GetMatchingFilter(MessageBuffer messageBuffer, out MessageFilter filter)
            {
                throw new NotImplementedException();
            }

            public bool GetMatchingFilters(Message message, ICollection<MessageFilter> results)
            {
                throw new NotImplementedException();
            }

            public bool GetMatchingFilters(MessageBuffer messageBuffer, ICollection<MessageFilter> results)
            {
                throw new NotImplementedException();
            }
        }

    }
}
