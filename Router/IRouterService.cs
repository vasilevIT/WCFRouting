using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    [ServiceContract]
    public interface IRouterService
    {
        [OperationContract]
        bool SendData(long a,long b,long c);

        [OperationContract]
        void setColpleteTask(Guid guid);

        [OperationContract]
        void setGuid(int type_task, Uri server,Guid guid);
    }
}
