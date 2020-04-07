namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models;

    using static RestaurantsPlatform.Common.GlobalConstants;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserAsync(userManager, UserEmail, UserPassword);
            await SeedUserAsync(userManager, AdministratorEmail, AdministratorPassword);
            await SeedUserAsync(userManager, RestaurantEmail, RestaurantPassword);
            await SeedUserAsync(userManager, SecondRestaurantEmail, SecondRestaurantPassword);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ApplicationUser userToSignIn = new ApplicationUser()
                {
                    Email = email,
                    UserName = email,
                };

                var result = await userManager.CreateAsync(userToSignIn, password);

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
