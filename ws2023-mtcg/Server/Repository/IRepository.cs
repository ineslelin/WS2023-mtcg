using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ws2023_mtcg.Server.Repository
{
    public interface IRepository<T>
    {
        T Read(string value);
        //void Create(T2 t);
        //void Update(T t);
        //void Delete(T t);
    }
}
