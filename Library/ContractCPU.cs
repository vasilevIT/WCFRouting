using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [ServiceContract]
    public interface ContractCPU
    {
        [OperationContract]
        long LongSum(long N);

        [OperationContract]
        double LongDiv(long X);
    }
}
