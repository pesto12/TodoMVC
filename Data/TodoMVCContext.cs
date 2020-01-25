using Microsoft.EntityFrameworkCore;
using TodoMVC.Models;

namespace TodoMVC.Data
{
    public class TodoMVCContext : DbContext
    {
        public TodoMVCContext (DbContextOptions<TodoMVCContext> options)
            : base(options)
        {
        }

        public DbSet<Topic> Topic { get; set; }

        public DbSet<TodoMVC.Models.TodoTask> TodoTask { get; set; }
    }
}