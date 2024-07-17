using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Framework.Constats;
using Entity_Framework.Entity;
using Microsoft.EntityFrameworkCore;
namespace Entity_Framework.Contexts
{
    internal class MyAppDbContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(SQLConnectionString.ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().Property(x=> x.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Student>().Property(x => x.IsDeleted).HasDefaultValue(false);
            modelBuilder.Entity<Teacher>().Property(x => x.IsDeleted).HasDefaultValue(false);

            modelBuilder.Entity<Group>().HasQueryFilter(x => x.IsDeleted == false);
            modelBuilder.Entity<Student>().HasQueryFilter(x => x.IsDeleted == false);
            modelBuilder.Entity<Teacher>().HasQueryFilter(x => x.IsDeleted == false);

        }
    }
}
