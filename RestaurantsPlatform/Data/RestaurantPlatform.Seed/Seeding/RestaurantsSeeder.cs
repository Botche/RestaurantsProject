// <copyright file="RestaurantsSeeder.cs" company="RestaurantsPlatform">
// Copyright (c) RestaurantsPlatform. All Rights Reserved.
// </copyright>

namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Data.Common.Seeding.Restaurants.SeedInfo;

    /// <summary>
    /// Category seeder.
    /// </summary>
    public class RestaurantsSeeder : ISeeder
    {
        /// <summary>
        /// Seeding method.
        /// </summary>
        /// <param name="dbContext">Database.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Restaurants.AnyAsync())
            {
                return;
            }

            string restaurantOwnerId = dbContext.Users
                .FirstOrDefault(category => category.Email == RestaurantEmail).Id;
            string secondRestaurantOwnerId = dbContext.Users
                .FirstOrDefault(category => category.Email == SecondRestaurantEmail).Id;
            string administratorId = dbContext.Users
                .FirstOrDefault(category => category.Email == AdministratorEmail).Id;

            int cafeteriaId = dbContext.Categories.FirstOrDefault(category => category.Name == "Cafeteria").Id;
            int casualDiningId = dbContext.Categories.FirstOrDefault(category => category.Name == "Casual dining").Id;
            int ethnicId = dbContext.Categories.FirstOrDefault(category => category.Name == "Ethnic").Id;
            int familyStyleId = dbContext.Categories.FirstOrDefault(category => category.Name == "Family style").Id;
            int fastFoodId = dbContext.Categories.FirstOrDefault(category => category.Name == "Fast food").Id;
            int fineDiningId = dbContext.Categories.FirstOrDefault(category => category.Name == "Fine dining").Id;
            int premiumCasualId = dbContext.Categories.FirstOrDefault(category => category.Name == "Premium casual").Id;
            int pubId = dbContext.Categories.FirstOrDefault(category => category.Name == "Pub").Id;

            List<(string RestaurantName, string Description, string Address, string OwnerName, string WorkingTime, string ContactInfo, int CategoryId, string UserId)> restaurants
                = new List<(string RestaurantName, string Description, string Address, string OwnerName, string WorkingTime, string ContactInfo, int CategoryId, string UserId)>
            {
                (FurnaName, FurnaDescription, FurnaAddress, FurnaOwnerName, FurnaWorkingTime, FurnaContactInfo, cafeteriaId, restaurantOwnerId),
                (RosicheName, RosicheDescription, RosicheAddress, RosicheOwnerName, RosicheWorkingTime, RosicheContactInfo, cafeteriaId, administratorId),
                (RainbowName, RainbowDescription, RainbowAddress, RainbowOwnerName, RainbowWorkingTime, RainbowContactInfo, cafeteriaId, secondRestaurantOwnerId),
                (MementoName, MementoDescription, MementoAddress, MementoOwnerName, MementoWorkingTime, MementoContactInfo, cafeteriaId, administratorId),

                (MomaName, MomaDescription, MomaAddress, MomaOwnerName, MomaWorkingTime, MomaContactInfo, casualDiningId, restaurantOwnerId),

                (IndianName, IndianDescription, IndianAddress, IndianOwnerName, IndianWorkingTime, IndianContactInfo, ethnicId, administratorId),
                (GurkhaName, GurkhaDescription, GurkhaAddress, GurkhaOwnerName, GurkhaWorkingTime, GurkhaContactInfo, ethnicId, restaurantOwnerId),

                (AladinName, AladinDescription, AladinAddress, AladinOwnerName, AladinWorkingTime, AladinContactInfo, fastFoodId, secondRestaurantOwnerId),
                (Aladin2Name, Aladin2Description, Aladin2Address, Aladin2OwnerName, Aladin2WorkingTime, Aladin2ContactInfo, fastFoodId, secondRestaurantOwnerId),
                (SkaptoName, SkaptoDescription, SkaptoAddress, SkaptoOwnerName, SkaptoWorkingTime, SkaptoContactInfo, fastFoodId, administratorId),
                (WokName, WokDescription, WokAddress, WokOwnerName, WokWorkingTime, WokContactInfo, fastFoodId, administratorId),

                (TenebrisName, TenebrisDescription, TenebrisAddress, TenebrisOwnerName, TenebrisWorkingTime, TenebrisContactInfo, fineDiningId, administratorId),
                (CosmosName, CosmosDescription, CosmosAddress, CosmosOwnerName, CosmosWorkingTime, CosmosContactInfo, fineDiningId, restaurantOwnerId),

                (BottegaName, BottegaDescription, BottegaAddress, BottegaOwnerName, BottegaWorkingTime, BottegaContactInfo, premiumCasualId, restaurantOwnerId),

                (SputnikName, SputnikDescription, SputnikAddress, SputnikOwnerName, SputnikWorkingTime, SputnikContactInfo, pubId, administratorId),
                (CocktailName, CocktailDescription, CocktailAddress, CocktailOwnerName, CocktailWorkingTime, CocktailContactInfo, pubId, restaurantOwnerId),
                (GastroName, GastroDescription, GastroAddress, GastroOwnerName, GastroWorkingTime, GastroContactInfo, pubId, administratorId),
                (RoadName, RoadDescription, RoadAddress, RoadOwnerName, RoadWorkingTime, RoadContactInfo, pubId, secondRestaurantOwnerId),
                (OscarName, OscarDescription, OscarAddress, OscarOwnerName, OscarWorkingTime, OscarContactInfo, pubId, secondRestaurantOwnerId),
            };

            foreach (var restaurant in restaurants)
            {
                await dbContext.Restaurants.AddAsync(new Restaurant
                {
                    RestaurantName = restaurant.RestaurantName,
                    Description = restaurant.Description,
                    Address = restaurant.Address,
                    OwnerName = restaurant.OwnerName,
                    WorkingTime = restaurant.WorkingTime,
                    ContactInfo = restaurant.ContactInfo,
                    CategoryId = restaurant.CategoryId,
                    UserId = restaurant.UserId,
                });
            }
        }
    }
}
