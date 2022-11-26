using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.DTO;
using ToDoList.Models;

namespace ToDoList.Profiles
{
    public class TodoProfile : AutoMapper.Profile
    {
        public TodoProfile()
        {
            CreateMap<TodoDTO, TodoModel>().ReverseMap();
        }
    }
}
