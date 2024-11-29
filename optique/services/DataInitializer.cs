using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using optique.Data;


public static class DataInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        if (serviceProvider.GetService<IServiceScopeFactory>() is IServiceScopeFactory serviceScopeFactory)
        {
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();

                // Initialize roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                InitializeRoles(roleManager).Wait();
            }
        }
    }

    private static async Task InitializeRoles(RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "ADMIN", "user", "SUPERADMIN", "DEFAULTROLE" }; 

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            var role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }
    }
}

}
