using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Router
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class RouterService : IRouterService
    {
        public bool SendData(long a, long b, long c)
        {
            Console.WriteLine("a = " + a + "; b = " + b);
            return true;
        }
    }
}
