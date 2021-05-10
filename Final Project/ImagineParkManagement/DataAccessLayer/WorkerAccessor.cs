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
    public class WorkerAccessor : IWorkerAccessor
    {
        public int DeactivateWorker(int? workerID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_safely_deactivate_worker", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);
            cmd.Parameters["@WorkerID"].Value = workerID;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException("Employee Could Not Be Deactivated.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int DeleteWorkerRole(int? workerID, string roleID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_safely_remove_workerrole", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);
            cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 25);
            cmd.Parameters["@WorkerID"].Value = workerID;
            cmd.Parameters["@RoleID"].Value = roleID;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException(roleID + " role could not be removed.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public int InsertNewWorker(Worker worker)
        {
            int workerID;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_insert_new_user", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 15);
            cmd.Parameters.Add("@StreetAddress", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@ZIPCode", SqlDbType.NVarChar, 5);

            cmd.Parameters["@Email"].Value = worker.Email;
            cmd.Parameters["@FirstName"].Value = worker.FirstName;
            cmd.Parameters["@LastName"].Value = worker.LastName;
            cmd.Parameters["@Phone"].Value = worker.Phone;
            cmd.Parameters["@StreetAddress"].Value = worker.StreetAddress;
            cmd.Parameters["@ZIPCode"].Value = worker.ZIPCode;

            try
            {
                conn.Open();
                workerID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return workerID;
        }

        public int InsertWorkerRole(int? workerID, string roleID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_add_workerrole", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);
            cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 25);

            cmd.Parameters["@WorkerID"].Value = workerID;
            cmd.Parameters["@RoleID"].Value = roleID;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException(roleID + " role could not be added.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return result;
        }

        public int ReactivateWorker(int? workerID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_reactivate_worker", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);

            cmd.Parameters["@WorkerID"].Value = workerID;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public List<string> SelectAllRoles()
        {
            List<string> roles = new List<string>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_all_roles", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return roles;
        }

        public List<int> SelectAvailabilityByWorkerID(int? workerID)
        {
            List<int> availability = new List<int>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_availability_by_id", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);
            cmd.Parameters["@WorkerID"].Value = workerID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        availability.Add(reader.GetInt32(0));
                        availability.Add(reader.GetInt32(1));
                        availability.Add(reader.GetInt32(2));
                        availability.Add(reader.GetInt32(3));
                        availability.Add(reader.GetInt32(4));
                        availability.Add(reader.GetInt32(5));
                        availability.Add(reader.GetInt32(6));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return availability;
        }

        public List<WorkerViewModel> SelectWorkersByActive(bool Active = true)
        {
            List<WorkerViewModel> workers = new List<WorkerViewModel>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_workers_by_active", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Active", SqlDbType.Bit);

            cmd.Parameters["@Active"].Value = Active;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var worker = new WorkerViewModel();
                        if (Active)
                        {
                            worker = new WorkerViewModel()
                            {
                                WorkerID = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                Phone = reader.GetString(4),
                                StreetAddress = reader.GetString(5),
                                ZIPCode = reader.GetString(6),
                                Active = reader.GetBoolean(7),
                                StartDate = reader.GetDateTime(8),
                                EndDate = DateTime.MinValue,
                                Roles = null,
                                Availability = null,
                                Projects = null
                            };
                        }
                        else
                        { 
                            worker = new WorkerViewModel()  
                            {
                                WorkerID = reader.GetInt32(0),
                                Email = reader.GetString(1),
                                FirstName = reader.GetString(2),
                                LastName = reader.GetString(3),
                                Phone = reader.GetString(4),
                                StreetAddress = reader.GetString(5),
                                ZIPCode = reader.GetString(6),
                                Active = reader.GetBoolean(7),
                                StartDate = reader.GetDateTime(8),
                                EndDate = reader.GetDateTime(9),
                                Roles = null,
                                Availability = null,
                                Projects = null
                            };
                        }
                        
                        workers.Add(worker);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Users List Not Found" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return workers;
        }

        public List<ProjectViewModel> SelectProjectsByWorkerID(int? workerID)
        {
            List<ProjectViewModel> projects = new List<ProjectViewModel>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_project_by_workerid", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);
            cmd.Parameters["@WorkerID"].Value = workerID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var project = new ProjectViewModel()
                        {
                            ProjectID = reader.GetInt32(0),
                            WorkerID = reader.GetInt32(1),
                            Paid = reader.GetBoolean(2),
                            // TaskListFilename = reader.GetString(3),
                            StartDate = reader.GetDateTime(4),
                            // Deadline = reader.GetDateTime(5),
                            EndDate = reader.GetDateTime(6),
                            // HoursWorked = reader.GetInt32(7),
                            ProjectName = reader.GetString(8),
                            ProjectDescription = reader.GetString(9)
                        };
                        projects.Add(project);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Projects List Not Found" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return projects;
        }

        public List<string> SelectRolesByWorkerID(int? workerID)
        {
            List<string> roles = new List<string>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_roles_by_workerid", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerId", SqlDbType.Int);

            cmd.Parameters["@WorkerID"].Value = workerID;

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Data Not Found" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return roles;
        }

        public int UpdateWorkerAvailability(int? workerID, List<int> oldDays, List<int> newDays)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_update_worker_availability", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);
            cmd.Parameters.Add("@NewSunday", SqlDbType.Int);
            cmd.Parameters.Add("@NewMonday", SqlDbType.Int);
            cmd.Parameters.Add("@NewTuesday", SqlDbType.Int);
            cmd.Parameters.Add("@NewWednesday", SqlDbType.Int);
            cmd.Parameters.Add("@NewThursday", SqlDbType.Int);
            cmd.Parameters.Add("@NewFriday", SqlDbType.Int);
            cmd.Parameters.Add("@NewSaturday", SqlDbType.Int);

            cmd.Parameters.Add("@OldSunday", SqlDbType.Int);
            cmd.Parameters.Add("@OldMonday", SqlDbType.Int);
            cmd.Parameters.Add("@OldTuesday", SqlDbType.Int);
            cmd.Parameters.Add("@OldWednesday", SqlDbType.Int);
            cmd.Parameters.Add("@OldThursday", SqlDbType.Int);
            cmd.Parameters.Add("@OldFriday", SqlDbType.Int);
            cmd.Parameters.Add("@OldSaturday", SqlDbType.Int);

            cmd.Parameters["@WorkerID"].Value = workerID;
            cmd.Parameters["@NewSunday"].Value = newDays[0];
            cmd.Parameters["@NewMonday"].Value = newDays[1];
            cmd.Parameters["@NewTuesday"].Value = newDays[2];
            cmd.Parameters["@NewWednesday"].Value = newDays[3];
            cmd.Parameters["@NewThursday"].Value = newDays[4];
            cmd.Parameters["@NewFriday"].Value = newDays[5];
            cmd.Parameters["@NewSaturday"].Value = newDays[6];

            cmd.Parameters["@OldSunday"].Value = oldDays[0];
            cmd.Parameters["@OldMonday"].Value = oldDays[1];
            cmd.Parameters["@OldTuesday"].Value = oldDays[2];
            cmd.Parameters["@OldWednesday"].Value = oldDays[3];
            cmd.Parameters["@OldThursday"].Value = oldDays[4];
            cmd.Parameters["@OldFriday"].Value = oldDays[5];
            cmd.Parameters["@OldSaturday"].Value = oldDays[6];

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Could Not Update Worker Availability", ex);
            }


            return result;
        }

        public int UpdateWorkerProfile(Worker oldWorker, Worker newWorker)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_update_worker_profile_data", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@WorkerID", SqlDbType.Int);
            cmd.Parameters.Add("@NewEmail", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewFirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@NewLastName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@NewPhone", SqlDbType.NVarChar, 15);
            cmd.Parameters.Add("@NewStreetAddress", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@NewZIPCode", SqlDbType.NVarChar, 5);

            cmd.Parameters.Add("@OldEmail", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@OldFirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@OldLastName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@OldPhone", SqlDbType.NVarChar, 15);
            cmd.Parameters.Add("@OldStreetAddress", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@OldZIPCode", SqlDbType.NVarChar, 5);

            cmd.Parameters["@WorkerID"].Value = oldWorker.WorkerID;
            cmd.Parameters["@NewEmail"].Value = newWorker.Email;
            cmd.Parameters["@NewFirstName"].Value = newWorker.FirstName;
            cmd.Parameters["@NewLastName"].Value = newWorker.LastName;
            cmd.Parameters["@NewPhone"].Value = newWorker.Phone;
            cmd.Parameters["@NewStreetAddress"].Value = newWorker.StreetAddress;
            cmd.Parameters["@NewZIPCode"].Value = newWorker.ZIPCode;

            cmd.Parameters["@OldEmail"].Value = oldWorker.Email;
            cmd.Parameters["@OldFirstName"].Value = oldWorker.FirstName;
            cmd.Parameters["@OldLastName"].Value = oldWorker.LastName;
            cmd.Parameters["@OldPhone"].Value = oldWorker.Phone;
            cmd.Parameters["@OldStreetAddress"].Value = oldWorker.StreetAddress;
            cmd.Parameters["@OldZIPCode"].Value = oldWorker.ZIPCode;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }


            return result;
        }

        public WorkerViewModel SelectWorkerByID(int? workerID)
        {
            WorkerViewModel worker = null;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_worker_by_id", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@WorkerID", workerID);

            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var tempWorker = new WorkerViewModel()
                        {
                            WorkerID = reader.GetInt32(0),
                            Email = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            Phone = reader.GetString(4),
                            StreetAddress = reader.GetString(5),
                            ZIPCode = reader.GetString(6),
                            Active = reader.GetBoolean(7),
                            StartDate = reader.GetDateTime(8),
                            EndDate = DateTime.MinValue,
                            Roles = null,
                            Availability = null,
                            Projects = null
                        };
                        worker = tempWorker;
                    }
                }
                reader.Close();
                
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Could Not Find Worker", ex);
            }
            finally
            {
                conn.Close();
            }
            return worker;
        }
    }
}
