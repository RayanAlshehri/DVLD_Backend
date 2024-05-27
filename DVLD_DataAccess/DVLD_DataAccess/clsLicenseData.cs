using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using CommonClasses;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace DVLD_DataAccess
{
    public static class clsLicenseData
    {
        public static bool GetLicenseByLicenseID(int LicenseID, ref int ApplicationID, ref int DriverID,  ref int ClassID,
            ref byte IssueReason, ref decimal PaidFee, ref DateTime IssueDate, ref DateTime ExpiryDate,
            ref int Constraints, ref bool IsActive, ref int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_GetLicenseByLicenseID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                ApplicationID = (int)Reader["ApplicationID"];
                                DriverID = (int)Reader["DriverID"];
                                ClassID = (int)Reader["ClassID"];
                                IssueReason = (byte)Reader["IssueReason"];
                                PaidFee = (decimal)Reader["PaidFee"];
                                IssueDate = (DateTime)Reader["IssueDate"];
                                ExpiryDate = (DateTime)Reader["ExpiryDate"];
                                Constraints = (int)Reader["Constraints"];
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
        public static bool GetLicenseByApplicationID(int ApplicationID, ref int LicenseID, ref int DriverID, ref int ClassID,
            ref byte IssueReason, ref decimal PaidFee, ref DateTime IssueDate, ref DateTime ExpiryDate,
            ref int Constraints, ref bool IsActive, ref int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_GetLicenseByApplicationID", Connection))
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
                                LicenseID = (int)Reader["LicenseID"];
                                DriverID = (int)Reader["DriverID"];
                                ClassID = (int)Reader["ClassID"];
                                IssueReason = (byte)Reader["IssueReason"];
                                PaidFee = (decimal)Reader["PaidFee"];
                                IssueDate = (DateTime)Reader["IssueDate"];
                                ExpiryDate = (DateTime)Reader["ExpiryDate"];
                                Constraints = (int)Reader["Constraints"];
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

        public static int AddNewLicense(int ApplicationID, int DriverID, int ClassID, byte IssueReason, decimal PaidFee, 
            DateTime ExpiryDate,int Constraints, bool IsActive, int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_AddNewLicense", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@ClassID", ClassID);
                    Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    Command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    Command.Parameters.AddWithValue("@PaidFee", PaidFee);
                    Command.Parameters.AddWithValue("@IssueDate", DateTime.Now);
                    Command.Parameters.AddWithValue("@ExpiryDate", ExpiryDate);
                    Command.Parameters.AddWithValue("@Constraints", Constraints);
                    Command.Parameters.AddWithValue("@IsActive", IsActive);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                    SqlParameter ReturnedValue = new SqlParameter("@NewLicenseID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    Command.Parameters.Add(ReturnedValue);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        object NewID = Command.Parameters["@NewLicenseID"].Value;

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

        public static bool UpdateLicense(int LicenseID,int ApplicationID, int DriverID, int ClassID, byte IssueReason, decimal PaidFee,
            DateTime IssueDate, DateTime ExpiryDate, int Constraints, bool IsActive, int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_UpdateLicense", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@ClassID", ClassID);
                    Command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    Command.Parameters.AddWithValue("@PaidFee", PaidFee);
                    Command.Parameters.AddWithValue("@IssueDate", IssueDate);
                    Command.Parameters.AddWithValue("@ExpiryDate", ExpiryDate);
                    Command.Parameters.AddWithValue("@Constraints", Constraints);
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

        public static int RenewLicense(int OldLicenseID, int PersonID, int DriverID, int ClassID, int Constraints, int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_RenewLicense", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@OldLicenseID", OldLicenseID);
                    Command.Parameters.AddWithValue("@LicensePersonID", PersonID);
                    Command.Parameters.AddWithValue("@LicenseDriverID", DriverID);
                    Command.Parameters.AddWithValue("@LicenseClassID", ClassID);
                    Command.Parameters.AddWithValue("@LicenseConstraints", Constraints);
                    Command.Parameters.AddWithValue("@RequestedByUserID", CreatedByUserID);

                    SqlParameter ReturnedValue = new SqlParameter("@NewLicenseID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    Command.Parameters.Add(ReturnedValue);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        object NewLicenseID = Command.Parameters["@NewLicenseID"].Value;

                        if (NewLicenseID != null)
                        {
                            return (int)NewLicenseID;
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

        public static int ReplaceLicense(int OldLicenseID, int PersonID, int DriverID, int ClassID, byte IssueReason, int Constraints, int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_ReplaceLicense", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@OldLicenseID", OldLicenseID);
                    Command.Parameters.AddWithValue("@LicensePersonID", PersonID);
                    Command.Parameters.AddWithValue("@LicenseDriverID", DriverID);
                    Command.Parameters.AddWithValue("@LicenseClassID", ClassID);
                    Command.Parameters.AddWithValue("@IssueReason", IssueReason);
                    Command.Parameters.AddWithValue("@LicenseConstraints", Constraints);
                    Command.Parameters.AddWithValue("@RequestedByUserID", CreatedByUserID);

                    SqlParameter ReturnedValue = new SqlParameter("@NewLicenseID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    Command.Parameters.Add(ReturnedValue);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        object NewLicenseID = Command.Parameters["@NewLicenseID"].Value;

                        if (NewLicenseID != null)
                        {
                            return (int)NewLicenseID;
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

        public static bool DoesLicenseExist(int LicenseID)
        {           
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_DoesLicenseExistByLicenseID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LicenseID", LicenseID);

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

        public static bool DoesLicenseExistByDriverIDandClass(int DriverID, int ClassID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_DoesLicenseExistByDriverIDAndClassID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@ClassID", ClassID);

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

            return  false;
        }

        public static DataTable GetAllDriverLicenses(int DriverID)
        {
            DataTable DT = new DataTable();

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_GetAllDriverLicenses", Connection))
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

        public static int[] GetAllActiveLocalDriverLicensesIDs(int DriverID)
        {
            List<int> list = new List<int>();

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Licenses.SP_GetAllActiveDriverLicensesIDs", Connection))
                {
                    Command.CommandType= CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                list.Add((int)Reader["LicenseID"]);
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                    }
                }
            }

            return list.ToArray();
        }
    }
}
