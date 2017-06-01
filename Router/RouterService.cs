using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Library;

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

        public void setColpleteTask(Guid guid)
        {
            //Найти самую старую задачу по заданному типу и серверу и удалить ее...
            CustomTask task = Program.tasks.ToList().Find(x => ((x.Guid == guid)
                                                                && !x.isCompleted()));
            Program.tasks.TryTake(out task);
            //task.setComplete();
        }

        public void setGuid(int type_task, Uri server,Guid guid)
        {
            CustomTask task = Program.tasks.ToList().Find(x => ((x.task_type == type_task)
                                                                && (x.working_server == server)
                                                                && !x.isCompleted()));
            if (task != null)
            {
                task.setGuid(guid);
            }
        }
    }
}
