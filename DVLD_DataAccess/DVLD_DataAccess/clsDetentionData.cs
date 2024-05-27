using System;
using System.Data;
using System.Data.SqlClient;
using CommonClasses;
using System.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;


namespace DVLD_DataAccess
{
    public static class clsDetentionData
    {
        public static bool GetDetentionByDetentionID(int DetentionID, ref int LicenseID, ref DateTime DetentionDate, ref decimal Fine,
             ref int CreatedByUserID, ref bool IsFinePaid, ref bool IsReleased, ref DateTime? ReleaseDate, 
             ref int? ReleaseApplicationID, ref int? ReleasedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicensesDetentions.SP_GetDetentionByDetentionID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DetentionID", DetentionID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                LicenseID = (int)Reader["LicenseID"];
                                DetentionDate = (DateTime)Reader["DetentionDate"];
                                Fine = (decimal)Reader["Fine"];
                                CreatedByUserID = (int)Reader["CreatedByUserID"];
                                IsFinePaid = (bool)Reader["IsFinePaid"];
                                IsReleased = (bool)Reader["IsReleased"];

                                if (Reader["ReleaseDate"] != DBNull.Value)
                                    ReleaseDate = (DateTime)Reader["ReleaseDate"];

                                if (Reader["ReleaseApplicationID"] != DBNull.Value)
                                    ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];

                                if (Reader["ReleasedByUserID"] != DBNull.Value)
                                    ReleasedByUserID = (int)Reader["ReleasedByUserID"];

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

        public static bool GetLastDetentionByLicenseID(int LicenseID, ref int DetentionID, ref DateTime DetentionDate, ref decimal Fine,
             ref int CreatedByUserID, ref bool IsFinePaid, ref bool IsReleased, ref DateTime? ReleaseDate,
             ref int? ReleaseApplicationID, ref int? ReleasedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicensesDetentions.SP_GetLastDetentionByLicenseID", Connection))
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
                                DetentionID = (int)Reader["DetentionID"];
                                DetentionDate = (DateTime)Reader["DetentionDate"];
                                Fine = (decimal)Reader["Fine"];
                                CreatedByUserID = (int)Reader["CreatedByUserID"];
                                IsFinePaid = (bool)Reader["IsFinePaid"];
                                IsReleased = (bool)Reader["IsReleased"];

                                if (Reader["ReleaseDate"] != DBNull.Value)
                                    ReleaseDate = (DateTime)Reader["ReleaseDate"];

                                if (Reader["ReleaseApplicationID"] != DBNull.Value)
                                    ReleaseApplicationID = (int)Reader["ReleaseApplicationID"];

                                if (Reader["ReleasedByUserID"] != DBNull.Value)
                                    ReleasedByUserID = (int)Reader["ReleasedByUserID"];

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

        public static int AddNewDetention(int LicenseID, DateTime DetentionDate, decimal Fine,
             int CreatedByUserID, bool IsFinePaid, bool IsReleased)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicensesDetentions.SP_AddNewDetention", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    Command.Parameters.AddWithValue("@DetentionDate", DetentionDate);
                    Command.Parameters.AddWithValue("@Fine", Fine);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    Command.Parameters.AddWithValue("@IsFinePaid", IsFinePaid);
                    Command.Parameters.AddWithValue("@IsReleased", IsReleased);

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

        public static bool UpdateDetention(int DetentionID, int LicenseID, DateTime DetentionDate, decimal Fine,
             int CreatedByUserID, bool IsFinePaid, bool IsReleased, DateTime? ReleaseDate,
             int? ReleaseApplicationID, int? ReleasedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicensesDetentions.SP_UpdateDetention", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DetentionID", DetentionID);
                    Command.Parameters.AddWithValue("@LicenseID", LicenseID);
                    Command.Parameters.AddWithValue("@DetentionDate", DetentionDate);
                    Command.Parameters.AddWithValue("@Fine", Fine);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    Command.Parameters.AddWithValue("@IsFinePaid", IsFinePaid);
                    Command.Parameters.AddWithValue("@IsReleased", IsReleased);
                    Command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
                    Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                    Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);

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

        public static bool MarkFineAsPaid(int DetentionID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                SqlCommand Command = new SqlCommand("LicensesDetentions.SP_MarkFeeAsPaid", Connection);

                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@DetentionID", DetentionID);

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

        public static bool ReleaseLicense(int DetentionID, int ReleasedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicensesDetentions.SP_ReleaseLicense", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DetainedLicenseID", DetentionID);
                    Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);

                    SqlParameter OutputParameter = new SqlParameter("@IsReleased", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    Command.Parameters.Add(OutputParameter);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        return (bool)Command.Parameters["@IsReleased"].Value;
                    }
                    catch (Exception EX)
                    {
                        clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                    }
                }
            }

            return false;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicensesDetentions.SP_IsLicenseDetained", Connection))
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

        public static DataTable GetAllDetentions()
        {
            DataTable DT = new DataTable();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicensesDetentions.SP_GetAllDetentions", Connection))
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

        public static int[] GetDriverDetentionsIDsWithUnpaidFines(int DriverID)
        {
            List<int> list = new List<int>();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicensesDetentions.SP_GetDriverDetentionsIDsWithUnpaidFines", Connection))
                {
                    Command.CommandType= CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        Connection.Open();

                        SqlDataReader Reader = Command.ExecuteReader();

                        while (Reader.Read())
                        {
                            list.Add((int)Reader["DetentionID"]);
                        }

                        Reader.Close();
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
