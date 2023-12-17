using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Repository
{
    internal interface IRepository<T, T2, T3>
    {
        T2 Read(T3 value);
        void Create(T t);
        void Update(T t);
        void Delete(T t);
    }
}
