using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Project
    {
        public int? ProjectID { get; set; }
        public int? WorkerID { get; set; }
        public Boolean Paid { get; set; }
        // public string TaskListFilename { get; set; }
        public DateTime StartDate { get; set; }
        // public DateTime? Deadline { get; set; }
        // public DateTime? EndDate { get; set; }
        // public int? HoursWorked { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
    }

    public class ProjectViewModel : Project
    {
        // public List<int> toolIDs { get; set; }
        // public List<string> toolNames { get; set; }
    }
}
