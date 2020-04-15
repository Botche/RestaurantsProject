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
                .FirstOrDefault(image => image.ImageUrl.Contains(CafeteriaName.ToLower().Replace(" ", "-")))
                .Id;
            int casualDiningId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(CasualDiningName.ToLower().Replace(" ", "-")))
                .Id;
            int ethnicId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(EthnicName.ToLower().Replace(" ", "-")))
                .Id;
            int familyStyleId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(FamilyStyleName.ToLower().Replace(" ", "-")))
                .Id;
            int fastFoodId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(FastFoodName.ToLower().Replace(" ", "-")))
                .Id;
            int fineDiningId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(FineDiningName.ToLower().Replace(" ", "-")))
                .Id;
            int premiumCasualId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(PremiumCasualName.ToLower().Replace(" ", "-")))
                .Id;
            int pubId = dbContext.CategoryImages
                .FirstOrDefault(image => image.ImageUrl.Contains(PubName.ToLower().Replace(" ", "-")))
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
