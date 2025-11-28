using apisRestNet.Api.Data;
using apisRestNet.Api.Model;

namespace apisRestNet.Api;

public static class SeedData
{
    public static void Initialize(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        //using var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (context.Users.Any())
            return; // Ya hay usuarios, no hace falta seed

        var users = new List<User>
        {
            new() { Username = "alice", Email = "alice@example.com" },
            new() { Username = "bob", Email = "bob@example.com" }
        };

        context.Users.AddRange(users);
        context.SaveChanges();
    }
}