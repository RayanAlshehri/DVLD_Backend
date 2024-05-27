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
    public static class clsDriverData
    {
        public static bool GetDriverByDriverID(int DriverID, ref int PersonID, ref string Password, ref int CreatedByUserID, ref DateTime CreationDate)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Drivers.SP_GetDriverByDriverID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                PersonID = (int)Reader["PersonID"];
                                CreatedByUserID = (int)Reader["CreatedByUserID"];
                                CreationDate = (DateTime)Reader["CreationDate"];
                                Password = Reader["Password"] != DBNull.Value ? Reader["Password"].ToString() : null;

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

        public static bool GetDriverByPersonID(int PersonID, ref int DriverID, ref string Password, ref int CreatedByUserID, 
            ref DateTime CreationDate)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Drivers.SP_GetDriverByPersonID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                DriverID = (int)Reader["DriverID"];
                                CreatedByUserID = (int)Reader["CreatedByUserID"];
                                CreationDate = (DateTime)Reader["CreationDate"];
                                Password = Reader["Password"] != DBNull.Value ? Reader["Password"].ToString() : null;

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

        public static int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreationDate)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Drivers.SP_AddNewDriver", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    Command.Parameters.AddWithValue("@CreationDate", CreationDate);

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

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreationDate)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Drivers.SP_UpdateDriver", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@PersonID", PersonID);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                    Command.Parameters.AddWithValue("@CreationDate", CreationDate);

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

        public static bool UpdatePassword(int DriverID, string Password)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Drivers.SP_UpdatePassword", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@Password", Password);

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

        public static bool DoesDriverExist(int DriverID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Drivers.SP_DoesDriverExistByDriverID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);

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

        public static bool DoesDriverExistByPersonID(int PersonID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Drivers.SP_DoesDriverExistByPersonID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static DataTable GetAllDrivers()
        {
            DataTable DT = new DataTable();

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Drivers.SP_GetAllDrivers", Connection))
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
