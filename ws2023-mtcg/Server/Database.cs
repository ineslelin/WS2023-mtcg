using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace ws2023_mtcg.Server
{
    internal class Database
    {
        private readonly string _connectionString = "Host=localhost;Database=mtcgdb;Username=admin;Password=1234";

        public Database()
        {
        
        }

        public IDbConnection InitDb()
        {
            var builder = new NpgsqlConnectionStringBuilder(_connectionString);

            string dbName = builder.Database;

            builder.Remove("Database");
            string cs = builder.ToString();

            IDbConnection connection = new NpgsqlConnection(cs);
            connection.Open();

            return connection;
        }
    }
}
