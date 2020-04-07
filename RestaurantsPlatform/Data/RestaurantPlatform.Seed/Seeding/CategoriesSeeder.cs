namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Data.Common.Seeding.Categories.SeedInfo;

    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            int cafeteriaId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl == "https://res.cloudinary.com/djlskbceh/image/upload/v1585916185/restaurant/categories/cafeteria/Cafeteria_ety9so.jpg")
                .Id;
            int casualDiningId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl == "https://res.cloudinary.com/djlskbceh/image/upload/v1586207894/restaurant/categories/casual-dining/CasualDining_hpng6o.jpg")
                .Id;
            int ethnicId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl == "https://res.cloudinary.com/djlskbceh/image/upload/v1586207925/restaurant/categories/ethnic/Ethnic_ere5s0.jpg")
                .Id;
            int familyStyleId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl == "https://res.cloudinary.com/djlskbceh/image/upload/v1586207928/restaurant/categories/family-style/FamilyStyle_brz7mq.jpg")
                .Id;
            int fastFoodId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl == "https://res.cloudinary.com/djlskbceh/image/upload/v1586207933/restaurant/categories/fast-food/FastFood_a5r5fa.jpg")
                .Id;
            int fineDiningId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl == "https://res.cloudinary.com/djlskbceh/image/upload/v1586207939/restaurant/categories/fine-dining/FineDining_lwyokf.jpg")
                .Id;
            int premiumCasualId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl == "https://res.cloudinary.com/djlskbceh/image/upload/v1586207943/restaurant/categories/premium-casual/PremiumCasual_xjynce.jpg")
                .Id;
            int pubId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl == "https://res.cloudinary.com/djlskbceh/image/upload/v1586207865/restaurant/categories/pub/Pub_ihqbd2.jpg")
                .Id;

            List <(string Name, int ImageId, string Description)> categories
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
