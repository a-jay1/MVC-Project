using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication6.Models
{
    public class CredentialsValidator
    {
        private readonly string _connectionString;

        string _email = "";

        public string getEmail ()
        {
            return this._email;
        }
        public CredentialsValidator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool IsValidCredentials(string email, string password)
        {
            return true;

            using (var connection = new SqlConnection(_connectionString))
            {
                int count = 0;
                try
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM Signup WHERE Email = @Email AND Password = @Password ";
                    //string query = "Insert signup(Email , Name , Password) VALUES (@Email , 'hai' , @Password) ";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    //command.ExecuteNonQuery();
                    count = (int)command.ExecuteScalar();
                    /*if(count > 0)
                    {
                        UserEmail object = new UserEmail(email);
                    }*/
                    this._email = email;
                }
                catch(Exception ex)
                {
                    throw new Exception("An error occurred.", ex);
                }

                return count > 0;
            }
        }

        public void insert(string str)
        {

            using(var connection = new SqlConnection(_connectionString))
            {

                try
                {
                    connection.Open();

                   // string query = "SELECT COUNT(*) FROM Signup WHERE Email = @Email AND Password = @Password ";
                    string query = "insert into dolist (email ,list) values (@Email ,@List ) ";
                    SqlCommand command = new SqlCommand(query, connection);
                    string email = "asd";
                    str = "asd";
                    //string list = "sleeping";
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@List", str);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred.", ex);
                }

            }
        }

    }

}