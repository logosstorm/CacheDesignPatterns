using Microsoft.EntityFrameworkCore;

namespace CacheApp.API.Models;

public class AppDbContext : DbContext

{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }


    public DbSet<Product> Products { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Pen 1" },
            new Product { Id = 2, Name = "Pen 2" },
            new Product { Id = 3, Name = "Pen 3" },
            new Product { Id = 4, Name = "Pen 4" }
        );


        base.OnModelCreating(modelBuilder);
    }
}