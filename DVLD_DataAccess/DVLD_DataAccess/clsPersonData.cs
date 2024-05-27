using System;
using System.Data;
using System.Data.SqlClient;
using CommonClasses;
using System.Configuration;
using System.Configuration.Provider;
using System.Text;
using System.Xml.Linq;

namespace DVLD_DataAccess
{
    public static class clsPersonData
    {
        public static bool GetPerson(int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
            ref char Gender, ref DateTime DateOfBirth, ref string NationalNumber, ref int NationalityID,
            ref string Phone, ref string Email, ref string Address, ref string ImagePath)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_GetPersonByPersonID", Connection))
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
                                FirstName = Reader["FirstName"].ToString();
                                SecondName = Reader["SecondName"].ToString();
                                ThirdName = Reader["ThirdName"] != DBNull.Value ? Reader["ThirdName"].ToString() : null;
                                LastName = Reader["LastName"].ToString();
                                Gender = Convert.ToChar(Reader["Gender"]);
                                DateOfBirth = (DateTime)Reader["DateOfBirth"];
                                NationalNumber = Reader["NationalNumber"].ToString();
                                NationalityID = (int)Reader["NationalityID"];
                                Phone = Reader["Phone"].ToString();
                                Email = Reader["Email"] != DBNull.Value ? Reader["Email"].ToString() : null;
                                Address = Reader["Address"].ToString();
                                ImagePath = Reader["ImagePath"] != DBNull.Value ? Reader["ImagePath"].ToString() : null;                             

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

        public static bool GetPerson(string NationalNumber, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName,
          ref string LastName, ref char Gender, ref DateTime DateOfBirth, ref int NationalityID,
          ref string Phone, ref string Email, ref string Address, ref string ImagePath)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_GetPersonByNationalNumber", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);

                    try
                    {
                        Connection.Open();

                        using (SqlDataReader Reader = Command.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                PersonID = (int)Reader["PersonID"];
                                FirstName = Reader["FirstName"].ToString();
                                SecondName = Reader["SecondName"].ToString();
                                ThirdName = Reader["ThirdName"] != DBNull.Value ? Reader["ThirdName"].ToString() : null;
                                LastName = Reader["LastName"].ToString();
                                Gender = Convert.ToChar(Reader["Gender"]);
                                DateOfBirth = (DateTime)Reader["DateOfBirth"];
                                NationalityID = (int)Reader["NationalityID"];
                                Phone = Reader["Phone"].ToString();
                                Email = Reader["Email"] != DBNull.Value ? Reader["Email"].ToString() : null;
                                Address = Reader["Address"].ToString();
                                ImagePath = Reader["ImagePath"] != DBNull.Value ? Reader["ImagePath"].ToString() : null;

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

        public static int AddNewPerson(string FirstName, string SecondName, string ThirdName, string LastName, char Gender,
            DateTime DateOfBirth, string NationalNumber, int NationalityID, string Phone, string Email, string Address, string ImagePath)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_AddNewPerson", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

                    Command.Parameters.AddWithValue("@FirstName", FirstName);
                    Command.Parameters.AddWithValue("@SecondName", SecondName);
                    Command.Parameters.AddWithValue("@ThirdName", ThirdName == null ? DBNull.Value : (object)ThirdName);
                    Command.Parameters.AddWithValue("@LastName", LastName);
                    Command.Parameters.AddWithValue("@Gender", Gender);
                    Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
                    Command.Parameters.AddWithValue("@NationalityID", NationalityID);
                    Command.Parameters.AddWithValue("@Phone", Phone);
                    Command.Parameters.AddWithValue("@Email", Email == null ? DBNull.Value : (object)Email);
                    Command.Parameters.AddWithValue("@Address", Address);                                      
                    Command.Parameters.AddWithValue("@ImagePath", ImagePath == null ? DBNull.Value : (object)ImagePath);
                   
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

        public static bool UpdatePerson(int PersonID, string FirstName, string SecondName, string ThirdName, string LastName, char Gender,
            DateTime DateOfBirth, string NationalNumber, int NationalityID, string Phone, string Email, string Address, string ImagePath)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_UpdatePerson", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;

                    Command.Parameters.AddWithValue("@PersonID", PersonID);
                    Command.Parameters.AddWithValue("@FirstName", FirstName);
                    Command.Parameters.AddWithValue("@SecondName", SecondName);
                    Command.Parameters.AddWithValue("@ThirdName", ThirdName == null ? DBNull.Value : (object)ThirdName);
                    Command.Parameters.AddWithValue("@LastName", LastName);
                    Command.Parameters.AddWithValue("@Gender", Gender);
                    Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                    Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
                    Command.Parameters.AddWithValue("@NationalityID", NationalityID);
                    Command.Parameters.AddWithValue("@Phone", Phone);
                    Command.Parameters.AddWithValue("@Email", Email == null ? DBNull.Value : (object)Email);
                    Command.Parameters.AddWithValue("@Address", Address);
                    Command.Parameters.AddWithValue("@ImagePath", ImagePath == null ? DBNull.Value : (object)ImagePath);

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

        public static bool DeletePerson(int PersonID)
        {          
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_DeletePersonByPersonID", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool DeletePerson(string NationalNumber)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_DeletePersonByNationalNmber", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);

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

        public static bool DoesPersonExist(int PersonID)
        {
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_DoesPersonExistByPersonID", Connection))
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

        public static bool DoesPersonExist(string NationalNumber)
        {           
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_DoesPersonExistByNationalNumber", Connection))
                {
                    Command.CommandType = CommandType.StoredProcedure;
                    Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);

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

        public static DataTable GetAllPersons()
        {
            DataTable DT = new DataTable();
            using (SqlConnection Connection = new SqlConnection(ConfigurationManager.AppSettings["DBConnectionString"]))
            {
                using (SqlCommand Command = new SqlCommand("Persons.SP_GetAllPersons", Connection))
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

