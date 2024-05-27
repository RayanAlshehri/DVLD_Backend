using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using CommonClasses;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Net;
using System.Security.Policy;

namespace DVLD_DataAccess
{
    public static class clsTestAppointmentData
    {
        public static bool GetAppointment(int AppointmentID, ref int TestType, ref int PersonID, ref int LocalLicenseApplicationID,
            ref decimal PaidFee, ref DateTime BookingDate, ref DateTime TestDate, ref bool IsTaken, 
            ref int CreatedByUserID, ref int? RetakeTestApplicationID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestsAppointments.SP_GetAppointmentByAppointmentID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                TestType = (int)Reader["TestType"];
                                PersonID = (int)Reader["PersonID"];
                                LocalLicenseApplicationID = (int)Reader["LocalLicenseApplicationID"];
                                PaidFee = (decimal)Reader["PaidFee"];
                                BookingDate = (DateTime)Reader["BookingDate"];
                                TestDate = (DateTime)Reader["TestDate"];
                                IsTaken = (bool)Reader["IsTaken"];
                                CreatedByUserID = (int)Reader["CreatedByUserID"];

                                if (Reader["RetakeTestApplicationID"] != DBNull.Value)
                                    RetakeTestApplicationID = (int)Reader["RetakeTestApplicationID"];

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

        public static bool GetLastAppointment(int LocalLicenseApplicationID, int TestType, ref int AppointmentID, ref int PersonID, 
            ref decimal PaidFee, ref DateTime BookingDate, ref DateTime TestDate, ref bool IsTaken, 
            ref int CreatedByUserID, ref int? RetakeTestApplicationID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {               
                using (SqlCommand Command = new SqlCommand("TestsAppointments.SP_GetLastAppointmentForLocalLicenseApplicatonAndTestType", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                    Command.Parameters.AddWithValue("@TestType", TestType);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {

                            if (Reader.Read())
                            {
                                AppointmentID = (int)Reader["AppointmentID"];
                                PersonID = (int)Reader["PersonID"];
                                PaidFee = (decimal)Reader["PaidFee"];
                                BookingDate = (DateTime)Reader["BookingDate"];
                                TestDate = (DateTime)Reader["TestDate"];
                                IsTaken = (bool)Reader["IsTaken"];
                                CreatedByUserID = (int)Reader["CreatedByUserID"];

                                if (Reader["RetakeTestApplicationID"] != DBNull.Value)
                                    RetakeTestApplicationID = (int)Reader["RetakeTestApplicationID"];

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

        public static int AddNewAppointment(int TestType, int PersonID, int LocalLicenseApplicationID, decimal PaidFee, DateTime BookingDate,
            DateTime TestDate, bool IsTaken, int CreatedByUserID, int? RetakeTestApplicationID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestsAppointments.SP_AddNewAppointment", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);
                    Command.Parameters.AddWithValue("@TestType", TestType);
                    Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                    Command.Parameters.AddWithValue("@PaidFee", PaidFee);
                    Command.Parameters.AddWithValue("@BookingDate", BookingDate);
                    Command.Parameters.AddWithValue("@TestDate", TestDate);
                    Command.Parameters.AddWithValue("@IsTaken", IsTaken);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID == null ? DBNull.Value : (object)RetakeTestApplicationID);

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

        public static bool UpdateAppointment(int AppointmentID, int TestType, int PersonID, int LocalLicenseApplicationID, decimal PaidFee, 
            DateTime BookingDate, DateTime TestDate, bool IsTaken, int CreatedByUserID, int? RetakeTestApplicationID)
        {            
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestsAppointments.SP_UpdateAppointment", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

                    Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    Command.Parameters.AddWithValue("@PersonID", PersonID);
                    Command.Parameters.AddWithValue("@TestType", TestType);
                    Command.Parameters.AddWithValue("@LocalLicenseApplicationID", LocalLicenseApplicationID);
                    Command.Parameters.AddWithValue("@PaidFee", PaidFee);
                    Command.Parameters.AddWithValue("@BookingDate", BookingDate);
                    Command.Parameters.AddWithValue("@TestDate", TestDate);
                    Command.Parameters.AddWithValue("@IsTaken", IsTaken);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID == null ? DBNull.Value : (object)RetakeTestApplicationID);

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

        public static bool UpdateAppointmentTestDate(int AppointmentID, DateTime TestDate)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestsAppointments.SP_UpdateAppointmentTestDate", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                    Command.Parameters.AddWithValue("@TestDate", TestDate);

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

        public static int[] GetUpcomingAppointmentsIDsForPerson(int PersonID)
        {
            List<int> list = new List<int>();

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestsAppointments.SP_GetUpcomingAppointmentsIDsForPerson", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                list.Add((int)Reader["AppointmentID"]);
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                    }
                }

                return list.ToArray();
            }
        }

        public static int GetTodayUpcomingTheoryTestAppointmentIDForPerson(int PersonID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestsAppointments.SP_GetTodayUpcomingTheoryTestAppointmentIDForPerson", Connection))
                {
                    Command.CommandType= CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);

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
}
