namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models;

    using static RestaurantsPlatform.Common.GlobalConstants;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var usersFromDb = dbContext.Users.IgnoreQueryFilters().ToList();

            await SeedUserAsync(usersFromDb, userManager, UserEmail, UserUsername, UserPassword, UserRoleName);
            await SeedUserAsync(usersFromDb, userManager, AdministratorEmail, AdministratorUsername, AdministratorPassword, AdministratorRoleName);
            await SeedUserAsync(usersFromDb, userManager, RestaurantEmail, RestaurantUsername, RestaurantPassword, RestaurantRoleName);
            await SeedUserAsync(usersFromDb, userManager, SecondRestaurantEmail, SecondRestaurantUsername, SecondRestaurantPassword, RestaurantRoleName);
        }

        private static async Task SeedUserAsync(List<ApplicationUser> usersFromDb, UserManager<ApplicationUser> userManager, string email, string username, string password, string role)
        {
            var user = usersFromDb.FirstOrDefault(user => user.Email == email || user.UserName == username);
            if (user == null)
            {
                ApplicationUser userToSignIn = new ApplicationUser()
                {
                    Email = email,
                    UserName = username,
                };

                var result = await userManager.CreateAsync(userToSignIn, password);
                var addToRoleResult = await userManager.AddToRoleAsync(userToSignIn, role);

                if (!result.Succeeded || !addToRoleResult.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
