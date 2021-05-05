using DataAccessLayer;
using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class ProjectManager : IProjectManager
    {
        private IProjectAccessor _projectAccessor;

        public ProjectManager()
        {
            _projectAccessor = new ProjectAccessor();
        }
        public ProjectManager(IProjectAccessor projectAccessor)
        {
            _projectAccessor = projectAccessor;
        }
        public bool AddNewProject(ProjectViewModel project)
        {
            bool result = false;

            int newProjectID = _projectAccessor.AddProject(project);
            try
            {
                if (newProjectID == 0)
                {
                    throw new ApplicationException("New Project Was Not Added.");
                }
                result = true;
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Adding Project Failed.", ex);
            }


            return result;
        }

        public bool AddNewProjectName(string projectName, string projectDescription)
        {
            bool result = false;

            try
            {
                result = (1 == _projectAccessor.AddProjectName(projectName, projectDescription));
                if (result == false)
                {
                    throw new ApplicationException("New Project Type Not Added");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Could Not Create a New Type of Project.", ex);
            }

            return result;
        }

        public bool DeactivateProject(ProjectViewModel project)
        {
            bool result = false;

            try
            {
                result = (1 == _projectAccessor.DeactivateProject(project));
                if (result == false)
                {
                    throw new ApplicationException("Project Could Not Be Marked As Finished.");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Project Not Finished.", ex);
            }


            return result;
        }

        public bool EditProjectDetail(ProjectViewModel oldProject, ProjectViewModel newProject)
        {
            bool result = false;

            try
            {
                result = (1 == _projectAccessor.UpdateProject(oldProject, newProject));
                if (result == false)
                {
                    throw new ApplicationException("Project Data Not Changed");
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Update Failed.", ex);
            }

            return result;
        }

        public List<string> RetrieveAllProjectNames()
        {
            List<string> projectNames = null;

            try
            {
                projectNames = _projectAccessor.SelectAllProjectNames();
                if(projectNames == null)
                {
                    projectNames = new List<string>();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found.", ex);
            }

            return projectNames;
        }

        public List<ProjectViewModel> RetrieveAllProjects()
        {
            List<ProjectViewModel> projects = null;
            try
            {
                projects = _projectAccessor.SelectAllProjects();
                if (projects == null)
                {
                    projects = new List<ProjectViewModel>();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found.", ex);
            }

            return projects;
        }

        public ProjectViewModel RetrieveProjectByID(int? projectID)
        {
            ProjectViewModel project = null;
            try
            {
                project = _projectAccessor.SelectProjectByID(projectID);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return project;
        }

        public List<ProjectViewModel> RetrieveProjectsByWorkerID(int? workerID)
        {
            List<ProjectViewModel> projects = null;

            try
            {
                projects = _projectAccessor.SelectProjectsByWorkerID(workerID);
                if (projects == null)
                {
                    projects = new List<ProjectViewModel>();
                }
            }
            catch (Exception ex)
            {
                return projects;
                // throw new ApplicationException("Data Could Not Be Found.", ex);
            }

            return projects;
        }

        public List<int> RetrieveToolsByProjectID(int? projectID)
        {
            List<int> tools = null;

            try
            {
                tools = _projectAccessor.SelectToolsByProjectID(projectID);
                if (tools == null)
                {
                    tools = new List<int>();
                }
            }
            catch (Exception ex)
            {
                return tools;
               // throw new ApplicationException("Data Could Not Be Found.", ex);
            }

            return tools;
        }
    }
}
