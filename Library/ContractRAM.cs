﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    [ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface ContractRAM
    {
        [OperationContract]
        string createBigCollection(Guid id, int N);
    }
}
