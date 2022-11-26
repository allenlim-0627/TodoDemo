using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Context;
using ToDoList.Models;

namespace ToDoList.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDBContext _context;

        public TodoRepository(TodoDBContext context)
        {
            _context = context;
        }

        public List<TodoModel> OnGet()
        {
            return _context.Todos.ToList();
        }

        public List<TodoModel> OnGetByFilter(string name, string desc, DateTime dueDateStart, DateTime dueDateEnd, string status, string tag)
        {
            var todos = _context.Todos.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                todos = todos.Where(x => x.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(desc))
            {
                todos = todos.Where(x => x.Description.Contains(desc));
            }

            if (dueDateStart != null && dueDateStart != DateTime.MinValue && dueDateStart != DateTime.MaxValue
                && dueDateEnd != null && dueDateEnd != DateTime.MinValue && dueDateEnd != DateTime.MaxValue)
            {
                todos = todos.Where(x => dueDateStart >= x.DueDate && dueDateEnd <= x.DueDate);
            }

            if (!string.IsNullOrEmpty(status))
            {
                todos = todos.Where(x => x.Status.Contains(status));
            }

            if (!string.IsNullOrEmpty(tag))
            {
                todos = todos.Where(x => x.Tag.Contains(tag));
            }

            return todos.ToList();
        }

        public List<TodoModel> OnGetBySort(bool? nameASC, bool? descASC, bool? dueDateASC, bool? statusASC, bool? tagASC)
        {
            var todos = _context.Todos.AsQueryable();
            if (nameASC != null)
            {
                if(nameASC == true)
                    todos = todos.OrderBy(x => x.Name);
                else
                    todos = todos.OrderByDescending(x => x.Name);
            }

            if (descASC != null)
            {
                if (descASC == true)
                    todos = todos.OrderBy(x => x.Description);
                else
                    todos = todos.OrderByDescending(x => x.Description);
            }

            if (dueDateASC != null)
            {
                if (dueDateASC == true)
                    todos = todos.OrderBy(x => x.DueDate);
                else
                    todos = todos.OrderByDescending(x => x.DueDate);
            }

            if (statusASC != null)
            {
                if (statusASC == true)
                    todos = todos.OrderBy(x => x.Status);
                else
                    todos = todos.OrderByDescending(x => x.Status);
            }

            if (tagASC != null)
            {
                if (tagASC == true)
                    todos = todos.OrderBy(x => x.Tag);
                else
                    todos = todos.OrderByDescending(x => x.Tag);
            }

            return todos.ToList();
        }

        public TodoModel OnGet(int id)
        {
            return _context.Todos.FirstOrDefault(x => x.Id == id);
        }

        public int OnPost(TodoModel newTodo)
        {
            _context.Todos.Add(newTodo);
            return _context.SaveChanges();
        }

        public int OnPut(TodoModel newTodo)
        {
            var todo = _context.Todos.FirstOrDefault(x => x.Id == newTodo.Id);
            if (todo != null)
            {
                todo.Name = newTodo.Name;
                todo.Status = newTodo.Status;
                todo.Tag = newTodo.Tag;
                todo.Description = newTodo.Description;
                todo.DueDate = newTodo.DueDate;
            }
            else
            {
                return 0;
            }
            return _context.SaveChanges();
        }

        public int OnDelete(int id)
        {
            var todo = _context.Todos.FirstOrDefault(x => x.Id == id);

            if (todo != null)
            {
                _context.Todos.Remove(todo);
            }
            
            return _context.SaveChanges();
        }
    }
}
