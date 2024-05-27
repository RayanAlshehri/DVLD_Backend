using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using CommonClasses;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace DVLD_DataAccess
{
    public static class clsTestTypeData
    {
        public static DataTable GetAllTests()
        {
            DataTable DT = new DataTable();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestTypes.SP_GetAllTestTypes", Connection))
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

        public static bool GetTest(int TestTypeID, ref string TestName, ref decimal Fee)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestTypes.SP_GetTest", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        Connection.Open();

                        SqlDataReader Reader = Command.ExecuteReader();

                        if (Reader.Read())
                        {
                            TestName = Reader["TestName"].ToString();
                            Fee = (decimal)Reader["Fee"];

                            return true;
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

        public static bool UpdateTestType(int TestTypeID, string TestName, decimal Fee)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {              
                using (SqlCommand Command = new SqlCommand("TestTypes.SP_UpdateTest", Connection))
                {
                    Command.CommandType= CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                    Command.Parameters.AddWithValue("@TestName", TestName);
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

        public static decimal GetFee(int TestTypeID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TestTypes.SP_GetTestFee", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                    try
                    {
                        Connection.Open();

                        object Result = Command.ExecuteScalar();

                        if (Result != null)
                        {
                            return (decimal)Result;
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
    }
}
