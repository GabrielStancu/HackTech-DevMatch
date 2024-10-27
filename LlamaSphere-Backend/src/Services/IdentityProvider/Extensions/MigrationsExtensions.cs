using IdentityProvider.Data;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Extensions;

public static class MigrationsExtensions
{
    public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.Migrate();

        return app;
    }
}
