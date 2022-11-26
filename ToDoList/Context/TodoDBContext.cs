using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Context
{
    public class TodoDBContext : IdentityDbContext
    {
        public TodoDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TodoModel> Todos { get; set; }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TodoModel>().ToTable("TodoModel");
            modelBuilder.Entity<UserModel>().ToTable("UserModel");

        }
    }
}
