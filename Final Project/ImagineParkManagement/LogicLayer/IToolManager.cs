using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IToolManager
    {
        Tool RetrieveToolByID(int? toolID);
        bool AddNewTool(string toolDescription);
        bool AddToolToProject(int? projectID, int? toolID);
        bool RemoveToolFromProject(int? projectID, int? toolID);
        int RetrieveProjectByToolID(int? toolID);
        List<Tool> RetrieveAllTools();

    }
}
