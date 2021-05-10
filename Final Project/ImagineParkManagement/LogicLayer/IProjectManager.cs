using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IProjectManager
    {
        List<ProjectViewModel> RetrieveAllProjects();
        List<ProjectViewModel> RetrieveProjectsByWorkerID(int? workerID);
        List<string> RetrieveAllProjectNames();
        ProjectViewModel RetrieveProjectByID(int? projectID);
        List<int> RetrieveToolsByProjectID(int? projectID);
        bool EditProjectDetail(Project oldProject, Project newProject);
        bool AddNewProject(Project project);
        bool DeactivateProject(Project project);
        bool AddNewProjectName(string projectName, string projectDescription);
        
    }
}
