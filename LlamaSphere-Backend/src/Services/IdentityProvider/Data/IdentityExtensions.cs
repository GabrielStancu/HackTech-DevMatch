using IdentityProvider.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Data;

public static class IdentityExtensions
{
    public static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("IdentityDb");
        services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(
                configuration.GetConnectionString("IdentityDb")
            ));
        services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        Console.WriteLine("--> Attempting to apply migrations...");

        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        try
        {
            context?.Database.Migrate();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"--> Could not run migrations. Reason: {ex.Message}");
        }
    }
}