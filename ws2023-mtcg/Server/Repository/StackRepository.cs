using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Models;

namespace ws2023_mtcg.Server.Repository
{
    internal class StackRepository
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true";

        public Cards[] ReadByOwner(string username)
        {
            if (username == null)
                throw new ArgumentNullException("uid can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT * FROM stack";

                        DbCommands.AddParameterWithValue(command, "owner", DbType.String, username);
                        command.ExecuteNonQuery();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Cards> tempStack = new List<Cards> ();

                            while (reader.Read())
                            {
                                Cards card = new Cards
                                {
                                    Id = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Damage = reader.GetDouble(2),
                                    Element = (ElementType)reader.GetInt32(3),
                                    Type = (CardType)reader.GetInt32(4),
                                    Owner = reader.GetString(5),
                                };

                                tempStack.Add(card);
                            }

                            return tempStack.ToArray();
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

        public void Create(Cards card)
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"INSERT INTO stack (id, name, damage, element, cardtype, owner)
                                                VALUES (@id, @name, @damage, @element, @cardtype, @owner)";

                        DbCommands.AddParameterWithValue(command, "id", DbType.String, card.Id);
                        DbCommands.AddParameterWithValue(command, "name", DbType.String, card.Name);
                        DbCommands.AddParameterWithValue(command, "damage", DbType.Double, card.Damage);
                        DbCommands.AddParameterWithValue(command, "element", DbType.Int32, (int)card.Element);
                        DbCommands.AddParameterWithValue(command, "cardtype", DbType.Int32, (int)card.Type);
                        DbCommands.AddParameterWithValue(command, "owner", DbType.String, card.Owner);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }

        public void Update(Cards card)
        {
            if (card == null)
                throw new ArgumentException("id can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"UPDATE stack SET owner=@owner WHERE id=@id";

                        DbCommands.AddParameterWithValue(command, "id", DbType.String, card.Id);
                        DbCommands.AddParameterWithValue(command, "owner", DbType.String, card.Owner);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }

        public void Delete(Cards card)
        {
            
        }

        public Cards ReadById(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT id, name, damage, element, cardtype, owner FROM stack WHERE id=@id";

                        DbCommands.AddParameterWithValue(command, "id", DbType.String, id);

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Cards
                                {
                                    Id = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Damage = reader.GetDouble(2),
                                    Element = (ElementType)reader.GetInt32(3),
                                    Type = (CardType)reader.GetInt32(4),
                                    Owner = reader.GetString(5),
                                };
                            }
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
    }
}
