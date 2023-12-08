using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Repository
{
    // db connectionstring should be "Host=localhost;Database=mtcgdb;Username=admin;Password=1234"
    internal interface IRepository<T>
    {
        T Read(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T t);
    }
}
