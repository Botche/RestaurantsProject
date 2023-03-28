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

            await SeedUserAsync(
                usersFromDb,
                userManager,
                UserEmail,
                UserUsername,
                UserPassword,
                UserRoleName,
                "https://res.cloudinary.com/djlskbceh/image/upload/v1587766798/restaurant/seedInfo/users/24_Hearts_DP_Profile_Pictures_collection_2019_-facebookdp_17_jev8jr.jpg",
                this.imageService);

            await SeedUserAsync(
                usersFromDb,
                userManager,
                AdministratorEmail,
                AdministratorUsername,
                AdministratorPassword,
                AdministratorRoleName,
                "https://res.cloudinary.com/djlskbceh/image/upload/v1587766796/restaurant/seedInfo/users/photo-1529665253569-6d01c0eaf7b6_amvvzp.jpg",
                this.imageService);

            await SeedUserAsync(
                usersFromDb,
                userManager,
                RestaurantEmail,
                RestaurantUsername,
                RestaurantPassword,
                RestaurantRoleName,
                "https://res.cloudinary.com/djlskbceh/image/upload/v1587766796/restaurant/seedInfo/users/Screenshot_20190912-000604_Multi_Parallel-min_fopwmz.jpg",
                this.imageService);

            await SeedUserAsync(
                usersFromDb,
                userManager,
                SecondRestaurantEmail,
                SecondRestaurantUsername,
                SecondRestaurantPassword,
                RestaurantRoleName,
                DefaultProfilePicture,
                this.imageService);

            await SeedUserAsync(
                usersFromDb,
                userManager,
                RealUserEmailOne,
                RealUserUsernameOne,
                RealUserPasswordOne,
                UserRoleName,
                "https://res.cloudinary.com/djlskbceh/image/upload/v1587766793/restaurant/seedInfo/users/%D0%B8%D0%B7%D1%82%D0%B5%D0%B3%D0%BB%D0%B5%D0%BD_%D1%84%D0%B0%D0%B9%D0%BB_jms7b7.jpg",
                this.imageService);

            await SeedUserAsync(
                usersFromDb,
                userManager,
                RealUserEmailTwo,
                RealUserUsernameTwo,
                RealUserPasswordTwo,
                UserRoleName,
                "https://res.cloudinary.com/djlskbceh/image/upload/v1587766793/restaurant/seedInfo/users/%D0%B8%D0%B7%D1%82%D0%B5%D0%B3%D0%BB%D0%B5%D0%BD_%D1%84%D0%B0%D0%B9%D0%BB_1_dn3f7f.jpg",
                this.imageService);
        }

        private static async Task SeedUserAsync(List<ApplicationUser> usersFromDb, UserManager<ApplicationUser> userManager, string email, string username, string password, string role, string imageUrl, ICloudinaryImageService imageService)
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

                var image = await imageService.UploadUserImageToCloudinaryAsync(imageUrl);
                userToSignIn.PublicId = image.PublicId;
                userToSignIn.ImageUrl = image.ImageUrl;

                var result = await userManager.CreateAsync(userToSignIn, password);
                var addToRoleResult = await userManager.AddToRoleAsync(userToSignIn, role);

                if (!result.Succeeded || !addToRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
