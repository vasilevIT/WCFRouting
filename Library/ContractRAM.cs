using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [ServiceContract]
    interface ContractRAM : IInterface2
    {
        [OperationContract]
        void createBigCollection(int N);
    }
}
