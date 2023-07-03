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

        private static string _email = "";

        
        public CredentialsValidator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool IsValidCredentials(string email, string password)
        {
            //return true;

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
                    _email = email;
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
                    
                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@List", str);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred.", ex);
                }

            }
        }

        public List<string> getList()
        {
            List<string> list = new List<string>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT list FROM dolist WHERE Email = @Email ";
                    //string query = "Insert signup(Email , Name , Password) VALUES (@Email , 'hai' , @Password) ";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", _email);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string task = reader.GetString(0);

                        // Create a tuple and add it to the todoList
                        list.Add(task);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred.", ex);
                }

                return list;
            }
        }

        public void Delete(int id)
        {

            using (var connection = new SqlConnection(_connectionString))
            {

                try
                {
                    connection.Open();

                    // string query = "SELECT COUNT(*) FROM Signup WHERE Email = @Email AND Password = @Password ";
                    string query = "DELETE FROM dolist WHERE id = (SELECT id FROM dolist WHERE email = @email ORDER BY ID OFFSET @n ROWS FETCH NEXT 1 ROWS ONLY)";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@n", id);
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