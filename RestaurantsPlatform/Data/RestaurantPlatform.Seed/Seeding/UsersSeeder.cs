// <copyright file="UsersSeeder.cs" company="RestaurantsPlatform">
// Copyright (c) RestaurantsPlatform. All Rights Reserved.
// </copyright>

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
    using RestaurantsPlatform.Services.Data.Interfaces;
    using static RestaurantsPlatform.Common.GlobalConstants;

    /// <summary>
    /// Category seeder.
    /// </summary>
    public class UsersSeeder : ISeeder
    {
        private readonly ICloudinaryImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersSeeder"/> class.
        /// </summary>
        /// <param name="imageService">Image service.</param>
        public UsersSeeder(ICloudinaryImageService imageService)
        {
            this.imageService = imageService;
        }

        /// <summary>
        /// Seeding method.
        /// </summary>
        /// <param name="dbContext">Database.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var usersFromDb = dbContext.Users.IgnoreQueryFilters().ToList();

            await SeedUserAsync(usersFromDb, userManager, UserEmail, UserUsername, UserPassword, UserRoleName, this.imageService);
            await SeedUserAsync(usersFromDb, userManager, AdministratorEmail, AdministratorUsername, AdministratorPassword, AdministratorRoleName, this.imageService);
            await SeedUserAsync(usersFromDb, userManager, RestaurantEmail, RestaurantUsername, RestaurantPassword, RestaurantRoleName, this.imageService);
            await SeedUserAsync(usersFromDb, userManager, SecondRestaurantEmail, SecondRestaurantUsername, SecondRestaurantPassword, RestaurantRoleName, this.imageService);
            await SeedUserAsync(usersFromDb, userManager, RealUserEmailOne, RealUserUsernameOne, RealUserPasswordOne, UserRoleName, this.imageService);
            await SeedUserAsync(usersFromDb, userManager, RealUserEmailTwo, RealUserUsernameTwo, RealUserPasswordTwo, UserRoleName, this.imageService);
        }

        private static async Task SeedUserAsync(List<ApplicationUser> usersFromDb, UserManager<ApplicationUser> userManager, string email, string username, string password, string role, ICloudinaryImageService imageService)
        {
            var user = usersFromDb.FirstOrDefault(user => user.Email == email || user.UserName == username);
            if (user == null)
            {
                ApplicationUser userToSignIn = new ApplicationUser()
                {
                    Email = email,
                    UserName = username,
                    EmailConfirmed = true,
                };

                var image = await imageService.UploadUserImageToCloudinaryAsync(DefaultProfilePicture);
                userToSignIn.PublicId = image.PublicId;
                userToSignIn.ImageUrl = image.ImageUrl;

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
