using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project_manager_app.Enums;

namespace Project_manager_app.Classes
{
    public class Assignment
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; }
        public AssignmentStatus Status{ get; set; }
        public int ExpectedDuration { get; set; }
        public AssignmentPriority Priority { get; set; }

    }
}
