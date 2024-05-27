using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClasses;
using System.Configuration;

namespace DVLD_DataAccess
{
    public static class clsInternationalLicenseData
    {
        public static bool GetLicenseByInternationalLicenseID(int InternationalLicenseID, ref int ApplicationID, ref int DriverID,
            ref int IssuedUsingLocalLicenseID, ref DateTime IssueDate, ref DateTime ExpiryDate, ref bool IsActive,
            ref int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("InternationalLicenses.SP_GetLicenseByInternationalLicenseID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                ApplicationID = (int)Reader["ApplicationID"];
                                DriverID = (int)Reader["DriverID"];
                                IssuedUsingLocalLicenseID = (int)Reader["IssuedUsingLocalLicenseID"];
                                IssueDate = (DateTime)Reader["IssueDate"];
                                ExpiryDate = (DateTime)Reader["ExpiryDate"];
                                IsActive = (bool)Reader["IsActive"];
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

        public static int AddNewLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate,
            DateTime ExpiryDate, bool IsActive, int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("InternationalLicenses.SP_AddNewLicense", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                    Command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    Command.Parameters.AddWithValue("@ExpiryDate", ExpiryDate);
                    Command.Parameters.AddWithValue("@IsActive", IsActive);
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

        public static int IssueLicense(int IssuedUsingLocalLicenseID, int PersonID, int DriverID, int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("InternationalLicenses.SP_IssueInternationalLicense", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LocalLicenseID", IssuedUsingLocalLicenseID);
                    Command.Parameters.AddWithValue("@LicensePersonID", PersonID);
                    Command.Parameters.AddWithValue("@LicenseDriverID", DriverID);
                    Command.Parameters.AddWithValue("@RequestedByUserID", CreatedByUserID);

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

        public static bool UpdateLicense(int InternationalLicenseID, int ApplicationID, int DriverID,
            int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpiryDate, bool IsActive,
            int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("InternationalLicenses.SP_UpdateInternationalLicense", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                    Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                    Command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    Command.Parameters.AddWithValue("@ExpiryDate", ExpiryDate);
                    Command.Parameters.AddWithValue("@IsActive", IsActive);
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
            }

            return false;
        }

        public static bool DoesLicenseExistByInternationalLicenseID(int InternationalLicenseID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("InternationalLicenses.SP_DoesLicenseExistByInternationalLicenseID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

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

                return false;
            }
        }

        public static bool DoesLicenseExistByLocalLicenseID(int LocalLicenseID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("InternationalLicenses.SP_DoesLicenseExistByLocalLicenseID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LocalLicenseID", LocalLicenseID);

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

                return false;
            }
        }

        public static DataTable GetAllDriverLicenses(int DriverID)
        {
            DataTable DT = new DataTable();

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("InternationalLicenses.SP_GetAllDriverLicenses", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static int GetActiveLicenseID(int DriverID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                SqlCommand Command = new SqlCommand("InternationalLicenses.SP_GetActiveLicenseIDForDriver", Connection);

                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@DriverID", DriverID);

                try
                {
                    Connection.Open();

                    object Result = Command.ExecuteScalar();

                    if (Result != null)
                    {
                        return (int)Result;
                    }
                }
                catch (Exception EX)
                {
                    clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                }
            }

            return -1;
        }
    }
}
