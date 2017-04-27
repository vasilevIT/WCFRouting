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
    }
}
