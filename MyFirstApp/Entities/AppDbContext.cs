using Microsoft.EntityFrameworkCore;

namespace MyFirstApp.Entities
{
    public class AppDbContext : DbContext
    {
        public DbSet<CalendarEntity> CalendarEntities { get; set; }
        public DbSet<NotesEntity> NotesEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=CalendarAppDb;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}

