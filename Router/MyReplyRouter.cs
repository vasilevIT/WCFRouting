using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Routing;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class MyReplyRouter : IRequestReplyRouter
    {
        public IAsyncResult BeginProcessRequest(Message message, AsyncCallback callback, object state)
        {
            Console.WriteLine("MyReplyRouter.BeginProcessRequest()");
            Console.WriteLine("Message = " + message.ToString());
            Console.WriteLine("callback = " + callback.Method.MemberType);
            throw new NotImplementedException();
        }

        public object AsyncMethodCaller { get; set; }

        public Message EndProcessRequest(IAsyncResult result)
        {
            Console.WriteLine("MyReplyRouter.EndProcessRequest()");
            throw new NotImplementedException();
        }
    }
}
