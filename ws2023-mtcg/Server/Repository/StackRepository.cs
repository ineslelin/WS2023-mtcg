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
    internal class StackRepository : IRepository<Cards, Cards[], int?>
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234";

        public Cards[] Read(int? uid)
        {
            if (uid == null)
                throw new ArgumentNullException("uid can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT * FROM stack INNER JOIN cards ON stack.f_cardid = cards.id WHERE stack.f_uid = @id";

                        DbCommands.AddParameterWithValue(command, "f_uid", DbType.Int32, uid);
                        command.ExecuteNonQuery();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Cards> tempStack = new List<Cards> ();

                            while (reader.Read())
                            {
                                Cards card = new Cards
                                {
                                    stackId = reader.GetInt32(0),
                                    OwnerId = reader.GetInt32(1),
                                    CardTypeId = reader.GetInt32(3), // that should be the id from cards table
                                    Name = reader.GetString(4), 
                                    Element = (ElementType)reader.GetInt32(5),
                                    Damage = reader.GetInt32(6),
                                    Type = (CardType)reader.GetInt32(7),
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

                        command.CommandText = @"INSERT INTO stack (f_uid, f_cardid) 
                                                VALUES (@f_uid, @f_cardid) RETURNING id";

                        DbCommands.AddParameterWithValue(command, "f_uid", DbType.String, card.OwnerId);
                        DbCommands.AddParameterWithValue(command, "f_cardid", DbType.String, card.CardTypeId);
                        command.ExecuteNonQuery();

                        card.stackId = Convert.ToInt32(command.ExecuteScalar() ?? 0);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }

        // there'S 100% errors in here i know them and ive seen them but also how am i supposed to debug if i dont have the cde necessary yet
        public void Update(Cards card)
        {
            if (card.Name == null)
                throw new ArgumentException("card name can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"UPDATE cards SET f_uid=@f_uid where id=@id";

                        DbCommands.AddParameterWithValue(command, "f_uid", DbType.Int32, card.OwnerId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }
        }

        // i also dont think ill actually need a delete cards function????? im like 99.9% sure i dont, i cant think of a single case in which id need it
        public void Delete(Cards card)
        {
            if (card.Name == null) // also gotta find a solution for this throw bc thats not how its supposed to be actually
                throw new ArgumentException("id can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"DELETE stack WHERE id=@id";

                        DbCommands.AddParameterWithValue(command, "id", DbType.Int32, card.stackId);
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
