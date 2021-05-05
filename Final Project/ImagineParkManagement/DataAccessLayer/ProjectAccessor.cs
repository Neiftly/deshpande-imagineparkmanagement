using DataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ProjectAccessor : IProjectAccessor
    {
        public int AddProject(ProjectViewModel project)
        {
            int projectID;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_add_project", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);
            cmd.Parameters.Add("@Paid", SqlDbType.Bit);
            cmd.Parameters.Add("@TaskListFilename", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime);
            cmd.Parameters.Add("@Deadline", SqlDbType.DateTime);
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);
            cmd.Parameters.Add("@HoursWorked", SqlDbType.Int);
            cmd.Parameters.Add("@ProjectName", SqlDbType.NVarChar, 256);

            cmd.Parameters["@WorkerID"].Value = project.WorkerID;
            cmd.Parameters["@Paid"].Value = project.Paid;
            cmd.Parameters["@TaskListFilename"].Value = "";
            cmd.Parameters["@StartDate"].Value = project.StartDate;
            cmd.Parameters["@DeadLine"].Value = DateTime.MaxValue;
            cmd.Parameters["@EndDate"].Value = DateTime.MaxValue;
            cmd.Parameters["@HoursWorked"].Value = 0;
            cmd.Parameters["@ProjectName"].Value = project.ProjectName;

            try
            {
                conn.Open();
                projectID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Project Could Not Be Created", ex);
            }
            finally
            {
                conn.Close();
            }
            return projectID;
        }
    

        public int AddProjectName(string projectName, string projectDescription)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_add_project_name", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ProjectName", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@ProjectDescription", SqlDbType.NVarChar, 256);

            cmd.Parameters["@ProjectName"].Value = projectName;
            cmd.Parameters["@ProjectDescription"].Value = projectDescription;
            

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Project Name Could Not Be Created", ex);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int DeactivateProject(ProjectViewModel project)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_finish_project", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ProjectID", SqlDbType.Int);
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime);
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime);

            cmd.Parameters["@ProjectID"].Value = project.ProjectID;
            cmd.Parameters["@StartDate"].Value = project.StartDate;
            cmd.Parameters["@EndDate"].Value = DateTime.Now;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Project Could Not Be Finished", ex);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        public List<string> SelectAllProjectNames()
        {
            List<string> projectNames = new List<string>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_all_project_names", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        projectNames.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return projectNames;
        }

        public List<ProjectViewModel> SelectAllProjects()
        {
            List<ProjectViewModel> projects = new List<ProjectViewModel>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_all_projects", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var tempProject = new ProjectViewModel()
                        {
                            ProjectID = reader.GetInt32(0),
                            WorkerID = reader.GetInt32(1),
                            Paid = reader.GetBoolean(2),
                            //TaskListFilename = reader.GetString(3),
                            StartDate = reader.GetDateTime(4),
                            // Deadline = reader.GetDateTime(5),
                            // EndDate = reader.GetDateTime(6),
                            // HoursWorked = (int)reader.GetInt32(7),
                            ProjectName = reader.GetString(5),
                            ProjectDescription = reader.GetString(6)


                        };
                        projects.Add(tempProject);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return projects;
        }

        public ProjectViewModel SelectProjectByID(int? projectID)
        {
            ProjectViewModel project = new ProjectViewModel();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_project_by_id", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ProjectID", SqlDbType.Int);
            cmd.Parameters["@ProjectID"].Value = projectID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var tempProject = new ProjectViewModel()
                        {
                            ProjectID = reader.GetInt32(0),
                            WorkerID = reader.GetInt32(1),
                            Paid = reader.GetBoolean(2),
                            // TaskListFilename = reader.GetString(3),
                            StartDate = reader.GetDateTime(4),
                            // Deadline = reader.GetDateTime(5),
                            // EndDate = reader.GetDateTime(6),
                            // HoursWorked = (int)reader.GetInt32(7),
                            ProjectName = reader.GetString(8),
                            ProjectDescription = reader.GetString(9)

                        };
                        project = tempProject;
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return project;
        }

        public List<ProjectViewModel> SelectProjectsByWorkerID(int? workerId)
        {
            List<ProjectViewModel> projects = new List<ProjectViewModel>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_project_by_worker_id", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);

            cmd.Parameters["@WorkerID"].Value = workerId;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var tempProject = new ProjectViewModel()
                        {
                            ProjectID = reader.GetInt32(0),
                            WorkerID = reader.GetInt32(1),
                            Paid = reader.GetBoolean(2),
                            // TaskListFilename = reader.GetString(3),
                            StartDate = reader.GetDateTime(4),
                            // Deadline = reader.GetDateTime(5),
                            // EndDate = reader.GetDateTime(6),
                            // HoursWorked = (int)reader.GetInt32(7),
                            ProjectName = reader.GetString(8),
                            ProjectDescription = reader.GetString(9)

                        };
                        projects.Add(tempProject);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return projects;
        }

        public int UpdateProject(ProjectViewModel oldProject, ProjectViewModel newProject)
        {
            int result;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_update_project", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ProjectID", SqlDbType.Int);
            cmd.Parameters.Add("@OldWorkerID", SqlDbType.Int);
            cmd.Parameters.Add("@OldPaid", SqlDbType.Bit);
            cmd.Parameters.Add("@OldTaskListFilename", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@OldStartDate", SqlDbType.DateTime);
            cmd.Parameters.Add("@OldDeadline", SqlDbType.DateTime);
            cmd.Parameters.Add("@OldEndDate", SqlDbType.DateTime);
            cmd.Parameters.Add("@OldHoursWorked", SqlDbType.Int);

            cmd.Parameters.Add("@NewWorkerID", SqlDbType.Int);
            cmd.Parameters.Add("@NewPaid", SqlDbType.Bit);
            cmd.Parameters.Add("@NewTaskListFilename", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@NewStartDate", SqlDbType.DateTime);
            cmd.Parameters.Add("@NewDeadline", SqlDbType.DateTime);
            cmd.Parameters.Add("@NewEndDate", SqlDbType.DateTime);
            cmd.Parameters.Add("@NewHoursWorked", SqlDbType.Int);

            cmd.Parameters["@ProjectID"].Value = oldProject.ProjectID;
            cmd.Parameters["@OldWorkerID"].Value = oldProject.WorkerID;
            cmd.Parameters["@OldPaid"].Value = oldProject.Paid;
            cmd.Parameters["@OldTaskListFilename"].Value = null;
            cmd.Parameters["@OldStartDate"].Value = oldProject.StartDate;
            cmd.Parameters["@OldDeadLine"].Value = null;
            cmd.Parameters["@OldEndDate"].Value = null;
            cmd.Parameters["@OldHoursWorked"].Value = null;

            cmd.Parameters["@NewWorkerID"].Value = newProject.WorkerID;
            cmd.Parameters["@NewPaid"].Value = newProject.Paid;
            cmd.Parameters["@NewTaskListFilename"].Value = null;
            cmd.Parameters["@NewStartDate"].Value = newProject.StartDate;
            cmd.Parameters["@NewDeadLine"].Value = null;
            cmd.Parameters["@NewEndDate"].Value = null;
            cmd.Parameters["@NewHoursWorked"].Value = null;


            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Project Could Not Be Updated ", ex);
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        public List<int> SelectToolsByProjectID(int? projectID)
        {
            List<int> toolIds = null;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_tool_by_projectid", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ProjectID", SqlDbType.Int);

            cmd.Parameters["@ProjectID"].Value = projectID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    toolIds = new List<int>();
                    while (reader.Read())
                    {
                        toolIds.Add(reader.GetInt32(0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }


            return toolIds;
        }
    }
}
