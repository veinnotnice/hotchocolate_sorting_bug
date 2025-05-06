using System;
using Microsoft.EntityFrameworkCore;

namespace hotchocolate_bug.DB
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Person> People { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).UseSerialColumn();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            });
        }

        internal bool? GetSortingContext()
        {
            throw new NotImplementedException();
        }
    }

    public class ContextFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("InMemoryBugDb")                // use in-memory DB
                .LogTo(Console.WriteLine, LogLevel.Information) // logs to console
                .EnableSensitiveDataLogging()                  // include parameter values
                .EnableDetailedErrors()                       // detailed error messages
                .Options;

            return new AppDbContext(options);
        }
    }
}
