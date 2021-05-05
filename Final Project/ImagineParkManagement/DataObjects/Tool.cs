using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Tool
    {
        public int? ToolID { get; set; }
        public string ToolDescription { get; set; }
    }

    public class ToolViewModel : Tool
    {
        public ToolViewModel(int? toolID, string toolDescription, int? projectID)
        {
            ToolID = toolID;
            ToolDescription = toolDescription;
            ProjectID = projectID;
        }

        public int? ProjectID { get; set; }
    }
}
