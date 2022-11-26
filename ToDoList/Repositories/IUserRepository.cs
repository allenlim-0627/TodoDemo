using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.DTO;

namespace ToDoList.Repositories
{
    public interface IUserRepository
    {
        bool GetToken(UserDTO user);
    }
}
