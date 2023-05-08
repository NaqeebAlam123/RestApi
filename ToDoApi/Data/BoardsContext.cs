using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Data
{
    public class BoardsContext : DbContext
    {
        public BoardsContext(DbContextOptions<BoardsContext> options) : base(options)
        {

        }
        public DbSet<Board> Boards { get; set; }
        public DbSet<ToDo> ToDos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>().HasMany(e=>e.ToDoList
            ).WithOne(e=>e.Board).HasForeignKey(e=>e.BoardId).IsRequired();
        }
    }
}
