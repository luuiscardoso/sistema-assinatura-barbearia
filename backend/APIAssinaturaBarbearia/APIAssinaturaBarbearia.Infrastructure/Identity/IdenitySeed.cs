namespace APIAssinaturaBarbearia.Infrastructure.Identity;

using APIAssinaturaBarbearia.Application.Options;
using APIAssinaturaBarbearia.Infrastructure.Identity.IdentityUsersUI;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class IdentitySeed
{   public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();
        var options = scope.ServiceProvider.GetRequiredService<IOptions<SeedOptions>>();

        const string adminRole = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        var adminUser = await userManager.FindByEmailAsync(options.Value.Email);

        if (adminUser == null)
        {
            adminUser = new Usuario
            {
                UserName = "admin",
                Email = options.Value.Email,
                Cpf = "12345678901",
                SecurityStamp = Guid.NewGuid().ToString(),
            };

            var result = await userManager.CreateAsync(adminUser, options.Value.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, adminRole);
            }
        }
    }
}

