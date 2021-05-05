using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IToolAccessor
    {
        Tool SelectToolByID(int? toolID);
        int AddNewTool(string toolDescription);
        int AddToolToProject(int? projectID, int? toolID);
        int RemoveToolFromProject(int? projectID, int? toolID);
        int SelectProjectIDByToolID(int? toolID);
        List<Tool> SelectAllTools();
    }
}
