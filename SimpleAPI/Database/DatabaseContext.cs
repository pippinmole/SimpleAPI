using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace SimpleAPI.Database;

public class TestModel {
    public int Id { get; set; }

    public string Value { get; set; }
}

public class DatabaseContext : DbContext {
    
    public DbSet<TestModel> TestModels { get; set; }
    
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // optionsBuilder.UseNpgsql("Host=127.0.0.1;Port=55298;Database=postgres;Username=postgres;Password=postgres");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestModel>()
            .HasKey(x => x.Id);
    }
}