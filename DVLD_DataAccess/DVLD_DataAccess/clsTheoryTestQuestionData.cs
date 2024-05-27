using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using CommonClasses;

namespace DVLD_DataAccess
{
    public static class clsTheoryTestQuestionData
    {
       public static int[] GetAllQuestionsIDsRandomOrder()
        {
            List<int> list = new List<int>();

            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {               
                using (SqlCommand Command = new SqlCommand("TheoryTestQuestions.SP_GetAllQuestionsIDsRandomOrder", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            while (Reader.Read())
                            {
                                list.Add((int)Reader["QuestionID"]);
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

        public static bool GetQuestion(int QuestionID, ref string QuestionText, ref string Answer1, ref string Answer2, ref string Answer3, ref int NumberOfCorrectAnswer, ref string ImagePath)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TheoryTestQuestions.SP_GetQuestion", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@QuestionID", QuestionID);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                QuestionText = Reader["QuestionText"].ToString();
                                Answer1 = Reader["Answer1"].ToString();
                                Answer2 = Reader["Answer2"].ToString();
                                Answer3 = Reader["Answer3"].ToString();
                                NumberOfCorrectAnswer = (int)Reader["NumberOfCorrectAnswer"];

                                if (Reader["ImagePath"] != DBNull.Value)
                                    ImagePath = Reader["ImagePath"].ToString();
                                else
                                    ImagePath = null;

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
        }

        public static int GetNumberQuestion()
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("TheoryTestQuestions.SP_GetNumberOfQuestions", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

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
