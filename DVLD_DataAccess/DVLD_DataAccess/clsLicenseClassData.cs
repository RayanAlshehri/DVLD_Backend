using System;
using System.Data;
using System.Data.SqlClient;
using CommonClasses;
using System.Configuration;

namespace DVLD_DataAccess
{
    public static class clsLicenseClassData
    {
        public static DataTable GetAllClasses()
        {
            DataTable DT = new DataTable();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicenseClasses.SP_GetAllLicenseClasses", Connection))
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

        public static bool GetClass(int ClassID, ref string ClassName, ref string ClassDescription, ref byte MinimumAge, 
            ref byte ValidityLength, ref decimal Fee)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicenseClasses.SP_GetLicenseClass", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ClassID", ClassID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                ClassName = Reader["ClassName"].ToString();
                                ClassDescription = Reader["ClassDescription"].ToString();
                                MinimumAge = (byte)Reader["MinimumAge"];
                                ValidityLength = (byte)Reader["ValidityLength"];
                                Fee = (decimal)Reader["Fee"];

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

        public static bool UpdateClass(int ClassID, string ClassName, string ClassDescription, byte MinimumAge, byte ValidityLength, decimal Fee)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("LicenseClasses.SP_UpdateLicenseClass", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@ClassID", ClassID);
                    Command.Parameters.AddWithValue("@ClassName", ClassName);
                    Command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                    Command.Parameters.AddWithValue("@MinimumAge", MinimumAge);
                    Command.Parameters.AddWithValue("@ValidityLength", ValidityLength);
                    Command.Parameters.AddWithValue("@Fee", Fee);

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
