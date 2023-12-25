using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Models;
using Npgsql;
using System.Linq.Expressions;

namespace ws2023_mtcg.Server.Repository
{
    internal class UserRepository : IRepository<User, User, string>
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true";

        public User Read(string username)
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

                        // GOTTA CREATE THE MONSTER AND SPELLCARD TABLES, THE STACK AND THE DECK AND STACK AND JOIN HERE
                        command.CommandText = @"SELECT id, username, password, coins, elo FROM users WHERE username=@username";

                        DbCommands.AddParameterWithValue(command, "username", DbType.String, username);
                        command.ExecuteNonQuery();

                        using(IDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                return new User()
                                {
                                    id = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Password = reader.GetString(2),
                                    Coins = reader.GetInt32(3),
                                    Elo = reader.GetInt32(4)
                                };
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
                using(IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using(IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"INSERT INTO users (username, password, coins, elo)
                                                VALUES (@username, @password, @coins, @elo) RETURNING id";

                        DbCommands.AddParameterWithValue(command, "username", DbType.String, user.Username);
                        DbCommands.AddParameterWithValue(command, "password", DbType.String, user.Password);
                        DbCommands.AddParameterWithValue(command, "coins", DbType.Int32, user.Coins);
                        DbCommands.AddParameterWithValue(command, "elo", DbType.Int32, user.Elo);
                        // command.ExecuteNonQuery();

                        user.id = Convert.ToInt32(command.ExecuteScalar() ?? 0);
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
            if(user.Username == null)
                throw new ArgumentException("id can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"UPDATE users SET coins=@coins, elo=@elo WHERE id=@id";

                        DbCommands.AddParameterWithValue(command, "coins", DbType.Int32, user.Coins);
                        DbCommands.AddParameterWithValue(command, "elo", DbType.Int32, user.Elo);
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

                        command.CommandText = @"DELETE users WHERE id=@id";

                        DbCommands.AddParameterWithValue(command, "id", DbType.Int32, user.id);
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
