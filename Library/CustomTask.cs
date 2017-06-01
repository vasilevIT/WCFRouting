using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class CustomTask : IEquatable<CustomTask>, IComparable<CustomTask>
    {
        private Guid id;
        private bool complete;
        public DateTime start { get; set; }
        public TimeSpan duration { get; set; }//in seconds
        public int task_type ;
        public Uri working_server { get; set; }

        public TaskInfo taskInfo = null;
        
        public CustomTask(TimeSpan dur, int type, Uri server)
        {
            id = Guid.NewGuid();
            complete = false;
            start = DateTime.Now;
            duration = dur;
            task_type = type;
            working_server = server;
        }

        public void setComplete()
        {
            complete = true;
        }

        public Guid Guid
        {
            get { return id; }
        }

        public bool isCompleted()
        {
            return complete;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            CustomTask objAsPart = obj as CustomTask;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public int CompareTo(CustomTask comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;

            else
                return this.start.CompareTo(comparePart.start);
        }
        public bool Equals(CustomTask other)
        {
            if (other == null) return false;
            return ((this.start.Equals(other.start))
                && (this.working_server.Equals(other.working_server))
                && (this.task_type.Equals(other.task_type)));
        }

        public void setGuid(Guid guid)
        {
            id = guid;
        }
        
    }
}
