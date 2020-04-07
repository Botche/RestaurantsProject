namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Data.Common.Seeding.CategoryImages.SeedInfo;

    public class CategoryImagesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.CategoryImages.Any())
            {
                return;
            }

            List<(string ImageUrl, string PublicId)> categoryImages
                = new List<(string ImageUrl, string PublicId)>
            {
                (Cafeteria, CafeteriaPublicId),
                (CasualDining, CasualDiningPublicId),
                (Ethnic, EthnicPublicId),
                (FamilyStyle, FamilyStylePublicId),
                (FastFood, FastFoodPublicId),
                (FineDining, FineDiningPublicId),
                (PremiumCasual, PremiumCasualPublicId),
                (Pub, PubPublicId),
            };

            foreach (var image in categoryImages)
            {
                await dbContext.CategoryImages.AddAsync(new CategoryImage
                {
                    ImageUrl = image.ImageUrl,
                    PublicId = image.PublicId,
                });
            }
        }
    }
}
