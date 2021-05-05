using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Worker
    {
        public int? WorkerID { get; set; }
        public string LastName { get;  set; }
        public string FirstName { get;  set; }
        public string StreetAddress { get;  set; }
        public string ZIPCode { get;  set; }
        public string Phone { get;  set; }
        public string Email { get;  set; }
        public bool Active { get;  set; }
        public DateTime StartDate { get;  set; }
        public DateTime EndDate { get;  set; }
    }

    public class WorkerViewModel : Worker
    {
        public List<string> Roles { get; set; }
        public List<ProjectViewModel> Projects { get; set; }
        public List<int> Availability { get; set; }
    }
}
