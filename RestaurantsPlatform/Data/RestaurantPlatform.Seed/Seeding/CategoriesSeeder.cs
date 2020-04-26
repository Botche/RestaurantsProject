// <copyright file="CategoriesSeeder.cs" company="RestaurantsPlatform">
// Copyright (c) RestaurantsPlatform. All Rights Reserved.
// </copyright>

namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Common.StringExtensions;
    using static RestaurantsPlatform.Data.Common.Seeding.Categories.SeedInfo;

    /// <summary>
    /// Category seeder.
    /// </summary>
    public class CategoriesSeeder : ISeeder
    {
        /// <summary>
        /// Seeding method.
        /// </summary>
        /// <param name="dbContext">Database to seed in.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            int cafeteriaId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(CafeteriaName.ToSlug()))
                .Id;
            int casualDiningId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(CasualDiningName.ToSlug()))
                .Id;
            int ethnicId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(EthnicName.ToSlug()))
                .Id;
            int familyStyleId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(FamilyStyleName.ToSlug()))
                .Id;
            int fastFoodId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(FastFoodName.ToSlug()))
                .Id;
            int fineDiningId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(FineDiningName.ToSlug()))
                .Id;
            int premiumCasualId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(PremiumCasualName.ToSlug()))
                .Id;
            int pubId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(PubName.ToSlug()))
                .Id;

            List<(string Name, int ImageId, string Description)> categories
                = new List<(string Name, int ImageId, string Description)>
            {
                (CafeteriaName, cafeteriaId, CafeteriaDescription),
                (CasualDiningName, casualDiningId, CasualDiningDescription),
                (EthnicName, ethnicId, EthnicDescription),
                (FamilyStyleName, familyStyleId, FamilyStyleDescription),
                (FastFoodName, fastFoodId, FastFoodDescription),
                (FineDiningName, fineDiningId, FineDiningDescription),
                (PremiumCasualName, premiumCasualId, PremiumCasualDescription),
                (PubName, pubId, PubDescription),
            };

            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    Name = category.Name,
                    Description = category.Description,
                    Title = category.Name,
                    ImageId = category.ImageId,
                });
            }
        }
    }
}
