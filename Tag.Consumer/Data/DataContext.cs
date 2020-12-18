using Microsoft.EntityFrameworkCore;
using Tag.Consumer.Models;
using TagConsumer.Models;

namespace TagConsumer.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Backup> Backups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite(@"Data Source=CustomerDB.db;");
        }
    }
}