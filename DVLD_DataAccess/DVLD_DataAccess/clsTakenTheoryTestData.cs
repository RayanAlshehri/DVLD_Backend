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
    public static class clsTakenTheoryTestData
    {
        public static bool GetTestByTheoryTestID(int TakenTheoryTestID, ref int TakenTestID, ref int Grade)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TakenTheoryTests.SP_GetTestByTakenTheoryTestID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TakenTheoryTestID", TakenTheoryTestID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                TakenTestID = (int)Reader["TakenTestID"];
                                Grade = (int)Reader["Grade"];

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

        public static bool GetTestByTakenTestID(int TakenTestID, ref int TakenTheoryTestID, ref int Grade)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TakenTheoryTests.SP_GetTestByTakenTestID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TakenTestID", TakenTestID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                TakenTheoryTestID = (int)Reader["TakenTheoryTestID"];
                                Grade = (int)Reader["Grade"];

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

        public static int AddNewTest(int TakenTestID, int Grade)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TakenTheoryTests.SP_AddNewTest", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@TakenTestID", TakenTestID);
                    Command.Parameters.AddWithValue("@Grade", Grade);


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

        public static bool UpdateTest(int TakenTheoryTestID, int TakenTestID, int Grade)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TakenTheoryTests.SP_UpdateTest", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

                    Command.Parameters.AddWithValue("@TakenTheoryTestID", TakenTheoryTestID);
                    Command.Parameters.AddWithValue("@TakenTestID", TakenTestID);
                    Command.Parameters.AddWithValue("@Grade", Grade);

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
    }
}
