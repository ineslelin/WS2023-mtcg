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
    internal class DeckRepository
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true";

        public Cards[] Read(string username)
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

                        command.CommandText = @"SELECT deck.id, stack.name, stack.damage, stack.element, stack.cardtype, stack.owner
                                                FROM deck INNER JOIN stack ON deck.id = stack.id WHERE deck.owner=@owner";

                        DbCommands.AddParameterWithValue(command, "owner", DbType.String, username);
                        command.ExecuteNonQuery();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<Cards> tempStack = new List<Cards>();

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

                        command.CommandText = @"INSERT INTO deck (id, owner)
                                                VALUES (@id, @owner)";

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

        public void Update(Cards card)
        {

        }

        public void Delete(Cards card)
        {

        }
    }
}
