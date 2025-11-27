using apisRestNet.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace apisRestNet.Api.Data;

public class AppDbContext : DbContext


{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<Rental>? Rentals { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>().Property(c => c.Price).HasPrecision(18, 2);
        base.OnModelCreating(modelBuilder);
    }
}