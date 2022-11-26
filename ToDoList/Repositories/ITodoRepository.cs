using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Repositories
{
    public interface ITodoRepository
    {
        List<TodoModel> OnGet();
        TodoModel OnGet(int id);
        int OnPost(TodoModel newTodo);
        int OnPut(TodoModel newTodo);
        int OnDelete(int id);
        List<TodoModel> OnGetByFilter(string name, string desc, DateTime dueDateStart, DateTime dueDateEnd, string status, string tag);
        List<TodoModel> OnGetBySort(bool? nameASC, bool? descASC, bool? dueDateASC, bool? statusASC, bool? tagASC);
    }
}
