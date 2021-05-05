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
        int UpdateProject(ProjectViewModel oldProject, ProjectViewModel newProject);
        int AddProject(ProjectViewModel project);
        int DeactivateProject(ProjectViewModel project);
        int AddProjectName(string projectName, string projectDescription);
        List<string> SelectAllProjectNames();
        List<int> SelectToolsByProjectID(int? projectID);
    }
}
