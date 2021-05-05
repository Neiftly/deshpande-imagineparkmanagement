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
    public class ToolAccessor : IToolAccessor
    {
        public int AddNewTool(string toolDescription)
        {
            int toolID = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_add_tool", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ToolDescription", SqlDbType.NVarChar, 256);

            cmd.Parameters["@ToolDescription"].Value = toolDescription;

            try
            {
                conn.Open();
                toolID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return toolID;
        }

        public int AddToolToProject(int? projectID, int? toolID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_insert_tool_into_project", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ProjectID", SqlDbType.Int);
            cmd.Parameters.Add("@ToolID", SqlDbType.Int);

            cmd.Parameters["@ProjectID"].Value = projectID;
            cmd.Parameters["@ToolID"].Value = toolID;

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

        public int RemoveToolFromProject(int? projectID, int? toolID)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_remove_tool_from_project", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ProjectID", SqlDbType.Int);
            cmd.Parameters.Add("@ToolID", SqlDbType.Int);

            cmd.Parameters["@ProjectID"].Value = projectID;
            cmd.Parameters["@ToolID"].Value = toolID;

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

        public List<Tool> SelectAllTools()
        {
            List<Tool> tools = new List<Tool>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_all_tools", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var tool = new Tool()
                        {
                            ToolID = reader.GetInt32(0),
                            ToolDescription = reader.GetString(1)
                        };
                        tools.Add(tool);
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
            return tools;
        }

        public int SelectProjectIDByToolID(int? toolID)
        {
            int projectID = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_get_projectid_by_toolid", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ToolID", SqlDbType.Int);

            cmd.Parameters["@ToolID"].Value = toolID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        projectID = reader.GetInt32(0);
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

            return projectID;
        }

        public Tool SelectToolByID(int? toolID)
        {
            Tool tool = null;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_tool_by_id", conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ToolID", SqlDbType.Int);

            cmd.Parameters["@ToolID"].Value = toolID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Tool tempTool = new Tool()
                        {
                            ToolID = reader.GetInt32(0),
                            ToolDescription = reader.GetString(1)
                        };

                        tool = tempTool;
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

            return tool;
        }
    }
}
