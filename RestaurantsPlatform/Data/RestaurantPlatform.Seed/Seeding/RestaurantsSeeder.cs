namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Data.Common.Seeding.Restaurants.SeedInfo;

    public class RestaurantsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Restaurants.Any())
            {
                return;
            }

            string restaurantOwnerId = dbContext.Users
                .FirstOrDefault(category => category.Email == "restaurantOwner@restaurantOwner.restaurantOwner").Id;
            string secondRestaurantOwnerId = dbContext.Users
                .FirstOrDefault(category => category.Email == "test@test.test").Id;
            string administratorId = dbContext.Users
                .FirstOrDefault(category => category.Email == "administrator@administrator.administrator").Id;

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
                (MomaName, MomaDescription, MomaAddress, MomaOwnerName, MomaWorkingTime, MomaContactInfo, casualDiningId, restaurantOwnerId),
                (IndianName, IndianDescription, IndianAddress, IndianOwnerName, IndianWorkingTime, IndianContactInfo, ethnicId, administratorId),
                (AladinName, AladinDescription, AladinAddress, AladinOwnerName, AladinWorkingTime, AladinContactInfo, fastFoodId, secondRestaurantOwnerId),
                (Aladin2Name, Aladin2Description, Aladin2Address, Aladin2OwnerName, Aladin2WorkingTime, Aladin2ContactInfo, fastFoodId, secondRestaurantOwnerId),
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
