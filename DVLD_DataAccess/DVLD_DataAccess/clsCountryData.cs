using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CommonClasses;
using System.Configuration;

namespace DVLD_DataAccess
{
    public static class clsCountryData
    {
        public static bool GetCountry(int CountryID, ref string CountryName)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                SqlCommand Command = new SqlCommand("Countries.SP_GetCountryByCountryID", Connection);

                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@CountryID", CountryID);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            CountryName = Reader["CountryName"].ToString();
                            return true;
                        }
                    }
                }
                catch (Exception EX)
                {
                    clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                }             
            }

            return false;
        }

        public static bool GetCountry(string CountryName, ref int CountryID)
        {           
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                SqlCommand Command = new SqlCommand("Countries.SP_GetCountryByCountryName", Connection);

                Command.CommandType = CommandType.StoredProcedure;
                Command.Parameters.AddWithValue("@CountryName", CountryName);

                try
                {
                    Connection.Open();

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            CountryID = (int)Reader["CountryID"];
                            return true;
                        }
                    }
                }
                catch (Exception EX)
                {
                    clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                }
            }

            return false;
        }

        public static DataTable GetAllCountries()
        {
            DataTable DT = new DataTable();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                SqlCommand Command = new SqlCommand("Countries.SP_GetAllCountries", Connection);

                Command.CommandType = CommandType.StoredProcedure;

                try
                {
                    Connection.Open();

                    SqlDataReader Reader = Command.ExecuteReader();

                    if (Reader.HasRows)
                    {
                        DT.Load(Reader);
                    }

                    Reader.Close();
                }
                catch (Exception EX)
                {
                    clsUtility.LogExceptionToEventViewer(ConfigurationManager.AppSettings["LoggedDatabaseExceptionSourceName"], EX);
                }             
            }

            return DT;
        }
    }
}
