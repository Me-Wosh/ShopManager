using Microsoft.EntityFrameworkCore;
using ShopManagerBackend.Entities.Configurations;

namespace ShopManagerBackend.Entities;

public class ShopManagerDbContext : DbContext
{
    public ShopManagerDbContext(DbContextOptions<ShopManagerDbContext> options) : base(options) {}

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductsConfiguration());
        modelBuilder.ApplyConfiguration(new UsersConfiguration());
    }
}