using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [ServiceContract]
    interface ContractCPU : IInterface2
    {
        [OperationContract]
        int LongSum(int N);
    }
}
