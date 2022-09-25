using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServelessFuncs
{
    public class Todo
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }   
    }
    public class TodoCreateModel
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
