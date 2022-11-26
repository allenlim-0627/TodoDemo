using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.DTO;
using ToDoList.Models;
using ToDoList.Repositories;

namespace ToDoList.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepo;
        private readonly IMapper _mapper;

        private readonly ILogger<TodoController> _logger;

        public TodoController(
            ILogger<TodoController> logger,
            ITodoRepository todoRepo,
            IMapper mapper)
        {
            _logger = logger;
            _todoRepo = todoRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// On Get All Todo List
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public IEnumerable<TodoDTO> OnGet()
        {
            IEnumerable<TodoModel> todoList = _todoRepo.OnGet();
            IEnumerable<TodoDTO> todoDTO = _mapper.Map<IEnumerable<TodoDTO>>(todoList);
            return todoDTO;
        }

        /// <summary>
        /// Create New Todo
        /// </summary>
        /// <remarks>If return 0, means empty request body</remarks>
        /// <remarks>else create new record successfully</remarks>
        [HttpPost]
        [Route("[action]")]
        public int OnPost(TodoDTO newTodo)
        {
            if (newTodo == null)
            {
                return 0;   
            }
            TodoModel todoDTO = _mapper.Map<TodoModel>(newTodo);
            return _todoRepo.OnPost(todoDTO);
        }

        /// <summary>
        /// Update Todo based on Id
        /// </summary>
        /// <remarks>If return 0, means empty request body or empty Id</remarks>
        /// <remarks>else update record successfully</remarks>
        [HttpPut]
        [Route("[action]")]
        public int OnPut(TodoDTO newTodo)
        {
            if (newTodo == null)
            {
                return 0;
            }

            if (newTodo.Id == 0)
            {
                return 0;
            }
            TodoModel todoDTO = _mapper.Map<TodoModel>(newTodo);
            return _todoRepo.OnPut(todoDTO);
        }

        /// <summary>
        /// Delete Todo based on Id
        /// </summary>
        /// <remarks>If return 0, means invalid Id</remarks>
        /// <remarks>else delete record successfully</remarks>
        [HttpDelete]
        [Route("[action]")]
        public int OnDelete(int id)
        {
            return _todoRepo.OnDelete(id);
        }

        /// <summary>
        /// Filter Todo List based on Multiple Fields
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public IEnumerable<TodoDTO> OnGetByFilter(string name, string desc, DateTime dueDateStart, DateTime dueDateEnd, string status, string tag)
        {
            var todoList = _todoRepo.OnGetByFilter(name, desc, dueDateStart, dueDateEnd, status, tag);
            IEnumerable<TodoDTO> todoDTO = _mapper.Map<IEnumerable<TodoDTO>>(todoList);
            return todoDTO;
        }

        /// <summary>
        /// Sort Todo List based on Multiple Fields
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        public IEnumerable<TodoDTO> OnGetBySort(bool? nameASC, bool? descASC, bool? dueDateASC, bool? statusASC, bool? tagASC)
        {
            var todoList = _todoRepo.OnGetBySort(nameASC, descASC, dueDateASC, statusASC, tagASC);
            IEnumerable<TodoDTO> todoDTO = _mapper.Map<IEnumerable<TodoDTO>>(todoList);
            return todoDTO;
        }
    }
}
