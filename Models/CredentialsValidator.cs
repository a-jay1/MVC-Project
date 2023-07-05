using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebApplication6.Models
{
    public class CredentialsValidator
    {
        private readonly string _connectionString;

        private static string _email = "asd@gmail.com";

        private static string _date = "";

        public CredentialsValidator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Set(string date)
        {
            _date = date;
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
                catch (Exception ex)
                {
                    throw new Exception("An error occurred.", ex);
                }

                return count > 0;
            }
        }

        public bool insertValid(List model)
        {
            
            using (var connection = new SqlConnection(_connectionString))
            {
                int count = 0;
                try
                {
                    connection.Open();

                    string query = "SELECT count(*) FROM dolist WHERE Email = @email AND _status = 0 AND _date = @date AND _start < @end AND _end > @start ";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@List", model.lists);
                    command.Parameters.AddWithValue("@Date", DateTime.Parse(_date));
                    command.Parameters.AddWithValue("@start", TimeSpan.Parse(model.startTime));
                    command.Parameters.AddWithValue("@end", TimeSpan.Parse(model.endTime));
                    count = (int)command.ExecuteScalar();
                    
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred.", ex);
                }

                return count < 1;
            }
        }
        public void insert(List model)
        {

            using (var connection = new SqlConnection(_connectionString))
            {

                try
                {
                    connection.Open();

                    // string query = "SELECT COUNT(*) FROM Signup WHERE Email = @Email AND Password = @Password ";
                    string query = "insert into dolist (email ,list,_date,_start,_end) values (@Email ,@List,@Date,@start,@end) ";
                    //string query = "insert into dolist (email ,list,_date) values (@Email ,@List,@Date) ";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@List", model.lists);
                    command.Parameters.AddWithValue("@Date", DateTime.Parse(_date));
                    command.Parameters.AddWithValue("@start", TimeSpan.Parse(model.startTime));
                    command.Parameters.AddWithValue("@end", TimeSpan.Parse(model.endTime));
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred.", ex);
                }

            }
        }

        /*public List<string> getList()
        {
            List<string> list = new List<string>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT list FROM dolist WHERE Email = @Email and _status = 0 and _date = @Date ORDER BY _start , _end , id" ;
                    //string query = "Insert signup(Email , Name , Password) VALUES (@Email , 'hai' , @Password) ";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@Date", _date);
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
        }*/
        public List<List<string>> getList()
        {
            List<List<string>> list = new List<List<string>>();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT list,_start,_end FROM dolist WHERE Email = @Email and _status = 0 and _date = @Date ORDER BY _start , _end , id";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@Date", _date);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string task = reader.GetString(0);
                        string startTime = reader.GetTimeSpan(1).ToString(@"hh\:mm\:ss");
                        string endTime = reader.GetTimeSpan(2).ToString(@"hh\:mm\:ss");

                        List<string> taskInfo = new List<string> { task, startTime, endTime };
                        list.Add(taskInfo);
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

        public List<List<string>> ViewCompleted()
        {
            List<List<string>> list = new List<List<string>>();

            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "SELECT list,_start,_end FROM dolist WHERE Email = @Email and _status = 1 and _date = @Date ORDER BY _start , _end , id";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@Date", _date);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        string task = reader.GetString(0);
                        string startTime = reader.GetTimeSpan(1).ToString(@"hh\:mm\:ss");
                        string endTime = reader.GetTimeSpan(2).ToString(@"hh\:mm\:ss");

                        List<string> taskInfo = new List<string> { task, startTime, endTime };
                        list.Add(taskInfo);
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
            string query = "";

            if (id-- == 1)
            {
                query = "UPDATE dolist SET _status = 2 WHERE id = (SELECT TOP 1 id FROM dolist WHERE email = @email and _status = 0 and _date = @Date ORDER BY _start , _end , id)";
            }
            else
            {
                query = "UPDATE dolist SET _status = 2 WHERE id = (SELECT id FROM dolist WHERE email = @email and _status = 0 and _date = @Date ORDER BY _start , _end , id OFFSET @n ROWS FETCH NEXT 1 ROWS ONLY)";
            }

            using (var connection = new SqlConnection(_connectionString))
            {

                try
                {
                    connection.Open();

                    //string query = "DELETE FROM dolist WHERE id = (SELECT id FROM dolist WHERE email = @email and _date = @Date ORDER BY ID OFFSET @n ROWS FETCH NEXT 1 ROWS ONLY)";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@Date", _date);
                    if (id != 0)
                    {
                        command.Parameters.AddWithValue("@n", id);
                    }
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred.", ex);
                }

            }
        }

        public void Completed(int id)
        {
            string query = "";

            if (id-- == 1)
            {
                query = "UPDATE dolist SET _status = 1 WHERE id = (SELECT TOP 1 id FROM dolist WHERE email = @email and _status = 0 and _date = @Date ORDER BY _start , _end , id)";
            }
            else
            {
                query = "UPDATE dolist SET _status = 1 WHERE id = (SELECT id FROM dolist WHERE email = @email and _status = 0 and _date = @Date ORDER BY _start , _end , id OFFSET @n ROWS FETCH NEXT 1 ROWS ONLY)";
            }

            using (var connection = new SqlConnection(_connectionString))
            {

                try
                {
                    connection.Open();

                    //string query = "DELETE FROM dolist WHERE id = (SELECT id FROM dolist WHERE email = @email and _date = @Date ORDER BY ID OFFSET @n ROWS FETCH NEXT 1 ROWS ONLY)";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@Email", _email);
                    command.Parameters.AddWithValue("@Date", _date);
                    if (id != 0)
                    {
                        command.Parameters.AddWithValue("@n", id);
                    }
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