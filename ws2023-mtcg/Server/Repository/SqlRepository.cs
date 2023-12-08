using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Repository
{
    internal class SqlRepository
    {
        private readonly string _connectionString;

        public SqlRepository(string connectionString)
        {
            this._connectionString = connectionString;
        }

        internal void Add()
        {

        }
    }
}
