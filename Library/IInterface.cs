using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Library
{
    [ServiceContract]
    public interface IInterface
    {
        [OperationContract]
        string Say(string str);

        [OperationContract]
        void Check();

        [OperationContract]
      //  [FaultContract(typeof(DivideByZeroException))] //Контракт сбоев
        MyMessage Calculate(MyMessage message);
    }

    [MessageContract]
    public class MyMessage
    {
        const int MAX_TTL = 5;
        private string operation;
        private int ttl;
        private double n1;
        private double n2;
        private double result;
        private string resultStr;
        private Guid messageId;

        public MyMessage()
        {
            ttl = MAX_TTL;
        }

        public MyMessage(string operation, double n1, double n2, double result)
        {
            ttl = MAX_TTL;
            this.operation = operation;
            this.n1 = n1;
            this.n2 = n2;
            this.result = result;
        }
        public MyMessage(MyMessage message)
        {
            ttl = MAX_TTL;
            this.operation = message.operation;
            this.n1 = message.n1;
            this.n2 = message.n2;
            this.result = message.result;
        }

        /*
        [MessageHeader]
        public int TTL {
            get { return ttl;  }
            set { ttl = value; }
        }
        */

        [MessageHeader]
        public string Operation {
            get { return operation;  }
            set { operation = value; }
        }

        [MessageBodyMember]
        public double N1
        {
            get { return n1; }
            set { n1 = value; }
        }
        [MessageBodyMember]
        public double N2
        {
            get { return n2; }
            set { n2 = value; }
        }

        [MessageBodyMember]
        public double Result
        {
            get { return result; }
            set { result = value; }
        }
        [MessageBodyMember]
        public string ResultStr
        {
            get { return resultStr; }
            set { resultStr = value; }
        }


        [MessageBodyMember]
        public Guid MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

    }

    [ServiceContract]
    public interface IInterface2 : IInterface, ContractCPU,ContractRAM
    {
        [OperationContract]
        string SayHello(string name);

        [OperationContract]
        Point Add(Point a, Point b);

        [OperationContract]
        string getHostName();
    }

    [DataContract(Namespace = "OtherNamespace")]
    public class Point
    {
        [DataMember]
        public double x;

        [DataMember]
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

    }
}
