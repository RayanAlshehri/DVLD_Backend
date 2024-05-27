using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClasses;

namespace DVLD_DataAccess
{
    public static class clsRegisteredVehicleData
    {
        public static bool GetRegisteredVehicle(int RegisteredVehicleID, ref int DriverID, ref string VehicleMake, ref string VehicleModel,
            ref int Year, ref int LicensePlateID, ref DateTime RegisterDate, ref int CreatedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("RegisteredVehicles.SP_GetRegisteredVehicleByRegisteredVehicleID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@RegisteredVehicleID", RegisteredVehicleID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                DriverID = (int)Reader["DriverID"];
                                VehicleMake = Reader["VehicleMake"].ToString();
                                VehicleModel = Reader["VehicleModel"].ToString();
                                Year = (int)Reader["Year"];
                                LicensePlateID = (int)Reader["LicensePlateID"];
                                RegisterDate = (DateTime)Reader["RegisterDate"];
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

        public static bool GetRegisteredVehicleByLicensePlateID(int LicensePlateID, ref int RegisteredVehicleID, ref int DriverID, ref string VehicleMake, ref string VehicleModel,
            ref int Year, ref DateTime RegisterDate, ref int CrearedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("RegisteredVehicles.SP_GetRegisteredVehicleByLicensePlateID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@LicensePlateID", LicensePlateID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                RegisteredVehicleID = (int)Reader["RegisteredVehicleID"];
                                DriverID = (int)Reader["DriverID"];
                                VehicleMake = Reader["VehicleMake"].ToString();
                                VehicleModel = Reader["VehicleModel"].ToString();                                
                                Year = (int)Reader["Year"];
                                RegisterDate = (DateTime)Reader["RegisterDate"];
                                CrearedByUserID = (int)Reader["CrearedByUserID"];

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

        public static int AddNewVehicle(int DriverID, string VehicleMake, string VehicleModel,
            int Year, ref int LicensePlateID, DateTime RegisterDate, int CrearedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("RegisteredVehicles.SP_AddNewVehicle", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@VehicleMake", VehicleMake);
                    Command.Parameters.AddWithValue("@VehicleModel", VehicleModel);                    
                    Command.Parameters.AddWithValue("@Year", Year);
                    Command.Parameters.AddWithValue("@RegisterDate", RegisterDate);
                    Command.Parameters.AddWithValue("@CreatedByUserID", CrearedByUserID);

                    SqlParameter OutputValue = new SqlParameter("@LicensePlateID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };

                    Command.Parameters.Add(OutputValue);

                    SqlParameter ReturnValue = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };

                    Command.Parameters.Add(ReturnValue);

                    try
                    {
                        Connection.Open();
                        Command.ExecuteNonQuery();

                        object NewLicensePlateID = Command.Parameters["@LicensePlateID"].Value; //Licens plate is generated in the database

                        if (NewLicensePlateID != null)
                        {
                            LicensePlateID = (int)NewLicensePlateID;
                        }

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

        public static bool UpdateVehcile(int RegisteredVehicleID, int DriverID, string VehicleMake, string VehicleModel,
            int Year, int LicensePlateID, DateTime RegisterDate, int CrearedByUserID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("RegisteredVehicles.SP_UpdateRegisteredVehicle", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@RegisteredVehicleID", RegisteredVehicleID);
                    Command.Parameters.AddWithValue("@DriverID", DriverID);
                    Command.Parameters.AddWithValue("@VehicleMake", VehicleMake);
                    Command.Parameters.AddWithValue("@VehicleModel", VehicleModel);                    
                    Command.Parameters.AddWithValue("@Year", Year);
                    Command.Parameters.AddWithValue("@LicensePlateID", LicensePlateID);
                    Command.Parameters.AddWithValue("@RegisterDate", RegisterDate);
                    Command.Parameters.AddWithValue("@CrearedByUserID", CrearedByUserID);

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

        public static DataTable GetAllVehiclesForDriver(int DriverID)
        {
            DataTable DT = new DataTable();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("RegisteredVehicles.SP_GetDriverVehicles", Connection))
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
    }
}
