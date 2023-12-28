using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;

namespace ws2023_mtcg.Server.Repository
{
    internal class UserProfileRepository
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true";

        public string Read(string username)
        {
            if (username == null)
                throw new ArgumentNullException("username can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT displayname, bio, image FROM userprofile WHERE username=@username";

                        DbCommands.AddParameterWithValue(command, "username", DbType.String, username);

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string profile = $"Name: {reader.GetString(0)}\n" +
                                                 $"Bio: {reader.GetString(1)}\n" +
                                                 $"Image: {reader.GetString(2)}";

                                return profile;
                            }

                            return null;
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            return null;
        }

        public void Create(User user)
        {
            if (Read(user.Username) != null)
            {
                throw new NpgsqlException("user already exists");
            }

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"INSERT INTO userprofile (username, displayname, bio, image) VALUES (@username, @displayname, @bio, @image)";

                        DbCommands.AddParameterWithValue(command, "username", DbType.String, user.Username);
                        DbCommands.AddParameterWithValue(command, "displayname", DbType.String, user.Name);
                        DbCommands.AddParameterWithValue(command, "bio", DbType.String, user.Bio);
                        DbCommands.AddParameterWithValue(command, "image", DbType.String, user.Image);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }

        public void Update(User user)
        {
            if (user.Username == null)
                throw new ArgumentException("id can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"UPDATE userprofile SET displayname=@displayname, bio=@bio, image=@image WHERE username=@username";

                        DbCommands.AddParameterWithValue(command, "username", DbType.String, user.Username);
                        DbCommands.AddParameterWithValue(command, "displayname", DbType.String, user.Name);
                        DbCommands.AddParameterWithValue(command, "bio", DbType.String, user.Bio);
                        DbCommands.AddParameterWithValue(command, "image", DbType.String, user.Image);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }

        public void Delete(User user)
        {
            if (user.Username == null)
                throw new ArgumentException("id can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"DELETE userprofile WHERE username=@username";

                        DbCommands.AddParameterWithValue(command, "username", DbType.String, user.Username);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }
    }
}
