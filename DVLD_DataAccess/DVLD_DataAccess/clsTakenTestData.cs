using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CommonClasses;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLD_DataAccess
{
    public static class clsTakenTestData
    {
        public static bool GetTest(int TakenTestID, ref int AppointmentID, ref bool Result, 
            ref string Notes, ref int? CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TakenTests.SP_GetTestByTakenTestID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TakenTestID", TakenTestID);

                    try
                    {
                        Connection.Open();

                        SqlDataReader Reader = Command.ExecuteReader();

                        if (Reader.Read())
                        {
                            AppointmentID = (int)Reader["AppointmentID"];
                            Notes = Reader["Notes"].ToString();
                            CreatedByUserID = Reader["CreatedByUserID"] != DBNull.Value ? (int?)Reader["CreatedByUserID"] : null;
                            Result = (bool)Reader["Result"];

                            return true;
                        }

                    }
                    catch (Exception EX)
                    {
                        clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                    }
                }
            }

            return false;
        }

        public static bool GetTestByAppointmentID(int AppointmentID, ref int TakenTestID, ref bool Result,
         ref string Notes, ref int? CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TakenTests.SP_GetTestByAppointmentID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    try
                    {
                        Connection.Open();

                        SqlDataReader Reader = Command.ExecuteReader();

                        if (Reader.Read())
                        {
                            TakenTestID = (int)Reader["TakenTestID"];
                            Notes = Reader["Notes"].ToString();
                            CreatedByUserID = Reader["CreatedByUserID"] != DBNull.Value ? (int?)Reader["CreatedByUserID"] : null;
                            Result = (bool)Reader["Result"];

                            return true;
                        }

                    }
                    catch (Exception EX)
                    {
                        clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                    }
                }
            }

            return false;
        }

        public static int AddNewTest(int AppointmentID, bool Result,
            string Notes, int? CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TakenTests.SP_AddNewTest", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    Command.Parameters.AddWithValue("@Result", Result);
                    Command.Parameters.AddWithValue("@Notes", Notes);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID != null ? (object)CreatedByUserID : DBNull.Value);

                    SqlParameter ReturnValue = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    Command.Parameters.Add(ReturnValue);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        object NewID = Command.Parameters["@NewID"].Value;

                        if (NewID != null)
                        {
                            return (int)NewID;
                        }
                    }
                    catch (Exception EX)
                    {
                        clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                    }
                }
            }

            return -1;
        }

        public static bool UpdateTest(int TakenTestID, int AppointmentID, bool Result,
            string Notes, int? CreatedByUserI)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TakenTests.SP_UpdateTest", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

                    Command.Parameters.AddWithValue("@TakenTestID", TakenTestID);
                    Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    Command.Parameters.AddWithValue("@Result", Result);
                    Command.Parameters.AddWithValue("@Notes", Notes);
                    Command.Parameters.AddWithValue("@CreatedByUserI", CreatedByUserI == null ? DBNull.Value : (object)CreatedByUserI);

                    try
                    {
                        Connection.Open();
                        return Command.ExecuteNonQuery() > 0;
                    }
                    catch (Exception EX)
                    {
                        clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                    }
                }

                return false;
            }
        }
    }
}
