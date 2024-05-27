using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using CommonClasses;


namespace DVLD_DataAccess
{
    public static class clsApplicationData
    {
        public static bool GetApplication(int ApplicationID,ref int PersonID, ref int ServiceID, ref byte ApplicationStatus,
            ref decimal PaidFee, ref DateTime ApplicationDate, ref DateTime LastStatusChangeDate, ref int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Applications.SP_GetApplication", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                PersonID = (int)Reader["PersonID"];
                                ServiceID = (int)Reader["ServiceID"];
                                ApplicationStatus = (byte)Reader["ApplicationStatus"];
                                PaidFee = (decimal)Reader["PaidFee"];
                                ApplicationDate = (DateTime)Reader["ApplicationDate"];
                                LastStatusChangeDate = (DateTime)Reader["LastStatusChangeDate"];
                                CreatedByUserID = (int)Reader["CreatedByUserID"];

                                return true;
                            }
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

        public static int AddNewApplication(int PersonID, int ServiceID, byte ApplicationStatus,
            decimal PaidFee, DateTime ApplicationDate, DateTime LastStatusChangeDate, int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Applications.SP_AddNewApplication", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);
                    Command.Parameters.AddWithValue("@ServiceID", ServiceID);
                    Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                    Command.Parameters.AddWithValue("@PaidFee", PaidFee);
                    Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                    Command.Parameters.AddWithValue("@LastStatusChangeDate", LastStatusChangeDate);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    SqlParameter ReturnValue = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    Command.Parameters.Add(ReturnValue);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        object NewlyInsertedID = Command.Parameters["@NewID"].Value;

                        if (NewlyInsertedID != null)
                        {
                            return (int)NewlyInsertedID;
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

        public static bool UpdateApplication(int ApplicationID,int PersonID, int ServiceID, byte ApplicationStatus,
            decimal PaidFee, DateTime ApplicationDate, DateTime LastStatusChangeDate, int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {               
                SqlCommand Command = new SqlCommand("Applications.SP_UpdateApplication", Connection);

                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@PersonID", PersonID);
                Command.Parameters.AddWithValue("@ServiceID", ServiceID);
                Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                Command.Parameters.AddWithValue("@PaidFee", PaidFee);
                Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                Command.Parameters.AddWithValue("@LastStatusChangeDate", LastStatusChangeDate);
                Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool ChangeApplicationStatus(int ApplicationID, byte ApplicationStatus)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                SqlCommand Command = new SqlCommand("Applications.SP_UpdateApplicationStatus", Connection);

                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);

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

        public static bool DeleteApplication(int ApplicationID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Applications.SP_DeleteApplication", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
            }

            return false;
        }
    }
}
