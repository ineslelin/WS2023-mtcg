using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ws2023_mtcg.Objects;

namespace ws2023_mtcg.Server.Repository
{
    internal class UserRepository : IRepository<User>
    {
        public User Read(string username)
        {
            User user = new User("aaaa");

            return user;
        }

        public void Create(User user)
        {

        }

        public void Update(User user)
        {

        }

        public void Delete(User user)
        {

        }
    }
}
