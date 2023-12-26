using Npgsql;
using System.Data;
using ws2023_mtcg.FightLogic.Enums;
using ws2023_mtcg.Models;

namespace ws2023_mtcg.Server.Repository
{
    internal class CardRepository : IRepository<Cards, Cards, string>
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234;Include Error Detail=true";

        public Cards Read(string id)
        {
            if (id == null)
                throw new ArgumentNullException("username can't be null");

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT name, damage, element, cardtype FROM cards WHERE id=@id";

                        DbCommands.AddParameterWithValue(command, "id", DbType.String, id);
                        command.ExecuteNonQuery();

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Cards()
                                {
                                    Id = reader.GetString(0),
                                    Name = reader.GetString(1),
                                    Damage = reader.GetDouble(2),
                                    Element = (ElementType)reader.GetInt32(3),
                                    Type = (CardType)reader.GetInt32(4)
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

        public void Create(Cards card)
        {
            if (Read(card.Id) != null)
            {
                throw new NpgsqlException("card id already exists");
            }

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"INSERT INTO cards (id, name, damage, element, cardtype, package)
                                                VALUES (@id, @name, @damage, @element, @cardtype, @package)";

                        DbCommands.AddParameterWithValue(command, "id", DbType.String, card.Id);
                        DbCommands.AddParameterWithValue(command, "name", DbType.String, card.Name);
                        DbCommands.AddParameterWithValue(command, "damage", DbType.Double, card.Damage);
                        DbCommands.AddParameterWithValue(command, "element", DbType.Int32, (int)card.Element);
                        DbCommands.AddParameterWithValue(command, "cardtype", DbType.Int32, (int)card.Type);
                        DbCommands.AddParameterWithValue(command, "package", DbType.Int32, card.Package);
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

        public int RetrieveHighestId()
        {
            int max = 0;

            try
            {
                using (IDbConnection connection = new NpgsqlConnection(_connectionString))
                {
                    using (IDbCommand command = connection.CreateCommand())
                    {
                        connection.Open();

                        command.CommandText = @"SELECT COALESCE(MAX(package), 0) FROM cards";

                        max = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Npgsql Error: {ex.Message}");
            }

            return max;
        }
    }
}

