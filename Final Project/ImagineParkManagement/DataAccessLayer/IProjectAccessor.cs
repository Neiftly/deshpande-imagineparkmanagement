using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IProjectAccessor
    {
        List<ProjectViewModel> SelectAllProjects();
        List<ProjectViewModel> SelectProjectsByWorkerID(int? workerId);
        ProjectViewModel SelectProjectByID(int? projectID);
        int UpdateProject(Project oldProject, Project newProject);
        int AddProject(Project project);
        int DeactivateProject(Project project);
        int AddProjectName(string projectName, string projectDescription);
        List<string> SelectAllProjectNames();
        List<int> SelectToolsByProjectID(int? projectID);
    }
}
