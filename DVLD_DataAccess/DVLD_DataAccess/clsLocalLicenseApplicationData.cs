using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CommonClasses;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLD_DataAccess
{
    public static class clsLocalLicenseApplicationData
    {
        public static bool GetApplication(int LocalLicenseApplicationID, ref int ApplicationID, 
            ref int LicenseClassID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LocalLicenseApplications.SP_GetLocalLicenseApplicationByLocalLicenseApplicationID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                ApplicationID = (int)Reader["ApplicationID"];
                                LicenseClassID = (int)Reader["LicenseClassID"];

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

        public static bool GetApplicationByApplicationID(int ApplicationID, ref int LocalLicenseApplicationID,
            ref int LicenseClassID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LocalLicenseApplications.SP_GetLocalLicenseApplicationByApplicationID", Connection))
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
                                LocalLicenseApplicationID = (int)Reader["LocalLicenseApplicationID"];
                                LicenseClassID = (int)Reader["LicenseClassID"];

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

        public static int AddNewApplication(int ApplicationID, int LicenseClassID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LocalLicenseApplications.SP_AddNewLocalLicenseApplication", Connection))
                {
                    Command.CommandType= CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    SqlParameter ReturnValue = new SqlParameter("@NewLocalLicenseApplicationID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    Command.Parameters.Add(ReturnValue);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        object NewID = Command.Parameters["@NewLocalLicenseApplicationID"].Value;
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

        public static bool UpdateApplication(int LocalLicenseApplicationID, int ApplicationID, 
            int LicenseClassID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LocalLicenseApplications.SP_UpdateLocalLicenseApplication", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                    Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static bool DoesPersonHaveNewApplicationForLicenseClass(int PersonID, int LicenseClassID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LocalLicenseApplications.SP_DoesPersonHaveNewApplicationForLicenseClass", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);
                    Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                    SqlParameter OutputParameter = new SqlParameter("@IsFound", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    Command.Parameters.Add(OutputParameter);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        if (Command.Parameters["@IsFound"].Value != DBNull.Value)
                        {
                            return (bool)Command.Parameters["@IsFound"].Value;
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

        public static DataTable GetAllApplications()
        {
            DataTable DT = new DataTable();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LocalLicenseApplications.SP_GetAllLocalLicenseApplications", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            DT.Load(Reader);
                        }
                    }
                    catch (Exception EX)
                    {
                        clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                    }
                }
            }

            return DT;
        }
    }
}
