using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Models;
using ws2023_mtcg.Server.Res;

namespace ws2023_mtcg.Server.Repository
{
    internal class TradingRepository
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true";

        public TradingDeal[] Read()
        {
            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT * FROM trading";
                        command.ExecuteNonQuery();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            List<TradingDeal> trades = new List<TradingDeal>();

                            while (reader.Read())
                            {
                                TradingDeal deal = new TradingDeal()
                                {
                                    Id = reader.GetString(0),
                                    CardToTrade = reader.GetString(1),
                                    Type = (CardType)reader.GetInt32(3),
                                    MinimumDamage = reader.GetInt32(4),
                                };

                                trades.Add(deal);
                            }

                            return trades.ToArray();
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

        public void Delete(string username)
        {
            if (username == null)
                throw new ArgumentException("id can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"DELETE FROM deck WHERE owner=@owner";

                        DbCommands.AddParameterWithValue(command, "owner", DbType.String, username);
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
