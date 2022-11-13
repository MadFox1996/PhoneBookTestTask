using Data.EntityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data
{
    public class PhoneBookContext:DbContext
    {
        public DbSet<PhoneBookEntry> PhoneBookEntries { get; set; }
        public DbSet<User> Users { get; set; }


        public PhoneBookContext()
        {

        }

        public PhoneBookContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasAlternateKey(u => u.Login);
        }
    }
}