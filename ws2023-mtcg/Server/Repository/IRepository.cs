using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Repository
{
    internal interface IRepository<T>
    {
        T Get(int id);

        void Add(T entity);

        void Update(T entity, string[] parameters);

        void Delete(int id);
    }
}
