namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    public class CategoryImagesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.CategoryImages.Any())
            {
                return;
            }

            string cafeteria = "https://res.cloudinary.com/djlskbceh/image/upload/v1585916185/restaurant/categories/cafeteria/Cafeteria_ety9so.jpg";
            string cafeteriaPublicId = "restaurant/categories/cafeteria/Cafeteria_ety9so";

            string casualDining = "https://res.cloudinary.com/djlskbceh/image/upload/v1586207894/restaurant/categories/casual-dining/CasualDining_hpng6o.jpg";
            string casualDiningPublicId = "restaurant/categories/casual-dining/CasualDining_hpng6o";

            string ethnic = "https://res.cloudinary.com/djlskbceh/image/upload/v1586207925/restaurant/categories/ethnic/Ethnic_ere5s0.jpg";
            string ethnicPublicId = "restaurant/categories/categories/ethnic/Ethnic_ere5s0";

            string familyStyle = "https://res.cloudinary.com/djlskbceh/image/upload/v1586207928/restaurant/categories/family-style/FamilyStyle_brz7mq.jpg";
            string familyStylePublicId = "restaurant/categories/family-style/FamilyStyle_brz7mq";

            string fastFood = "https://res.cloudinary.com/djlskbceh/image/upload/v1586207933/restaurant/categories/fast-food/FastFood_a5r5fa.jpg";
            string fastFoodPublicId = "restaurant/categories/fast-food/FastFood_a5r5fa";

            string fineDining = "https://res.cloudinary.com/djlskbceh/image/upload/v1586207939/restaurant/categories/fine-dining/FineDining_lwyokf.jpg";
            string fineDiningPublicId = "restaurant/categories/fine-dining/FineDining_lwyokf";

            string premiumCasual = "https://res.cloudinary.com/djlskbceh/image/upload/v1586207943/restaurant/categories/premium-casual/PremiumCasual_xjynce.jpg";
            string premiumCasualPublicId = "restaurant/categories/premium-casual/PremiumCasual_xjynce";

            string pub = "https://res.cloudinary.com/djlskbceh/image/upload/v1586207865/restaurant/categories/pub/Pub_ihqbd2.jpg";
            string pubPublicId = "restaurant/categories/pub/Pub_ihqbd2";


            List<(string ImageUrl, string PublicId)> categoryImages
                = new List<(string ImageUrl, string PublicId)>
            {
                (cafeteria, cafeteriaPublicId),
                (casualDining, casualDiningPublicId),
                (ethnic, ethnicPublicId),
                (familyStyle, familyStylePublicId),
                (fastFood, fastFoodPublicId),
                (fineDining, fineDiningPublicId),
                (premiumCasual, premiumCasualPublicId),
                (pub, pubPublicId),
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
