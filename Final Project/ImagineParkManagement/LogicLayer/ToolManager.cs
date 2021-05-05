using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class ToolManager : IToolManager
    {
        private IToolAccessor _toolAccessor;
        public ToolManager()
        {
            _toolAccessor = new ToolAccessor();
        }

        public ToolManager(IToolAccessor toolAccessor)
        {
            _toolAccessor = toolAccessor;
        }

        public bool AddNewTool(string toolDescription)
        {
            
            bool result = false;

            int toolID = _toolAccessor.AddNewTool(toolDescription);

            try
            {
                
                if (toolID == 0)
                {
                    throw new ApplicationException("Tool Not Added.");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Tool Not Created.", ex);
            }

            return result;

        }

        public bool AddToolToProject(int? projectID, int? toolID)
        {
            bool result = false;

            try
            {
                result = (1 == _toolAccessor.AddToolToProject(projectID, toolID));
                if (result == false)
                {
                    throw new ApplicationException("Tool Not Checked Out");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Tool Not Added To Project.", ex);
            }

            return result;
        }

        public bool RemoveToolFromProject(int? projectID, int? toolID)
        {
            bool result = false;

            try
            {
                result = (1 == _toolAccessor.RemoveToolFromProject(projectID, toolID));
                if (result == false)
                {
                    throw new ApplicationException("Tool Not Checked Back In");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Tool Not Removed From Project", ex);
            }

            return result;
        }

        public List<Tool> RetrieveAllTools()
        {
            List<Tool> tools = null;

            try
            {
                tools = _toolAccessor.SelectAllTools();
                if (tools == null)
                {
                    throw new ApplicationException("No Tools Found...");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found", ex);
            }

            return tools;
        }

        public int RetrieveProjectByToolID(int? toolID)
        {
            int projectID = 0;
            try
            {
                 projectID = _toolAccessor.SelectProjectIDByToolID(toolID);
                
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found.", ex);
            }

            return projectID;
        }

        public Tool RetrieveToolByID(int? toolID)
        {
            Tool tool = null;

            try
            {
                tool = _toolAccessor.SelectToolByID(toolID);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return tool;
        }
    }
}
