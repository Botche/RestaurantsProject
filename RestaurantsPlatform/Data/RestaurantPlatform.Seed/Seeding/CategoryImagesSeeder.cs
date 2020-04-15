namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    using static RestaurantsPlatform.Data.Common.Seeding.Categories.SeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.CategoryImages.SeedInfo;

    public class CategoryImagesSeeder : ISeeder
    {
        private readonly ICloudinaryImageService imageService;

        public CategoryImagesSeeder(ICloudinaryImageService imageService)
        {
            this.imageService = imageService;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.CategoryImages.Any())
            {
                return;
            }

            var cafeteriaImage = await this.UploadImagesToCloudinaryAsync(Cafeteria, CafeteriaName.ToLower().Replace(" ", "-"));
            var casualImage = await this.UploadImagesToCloudinaryAsync(CasualDining, CasualDiningName.ToLower().Replace(" ", "-"));
            var ethnicImage = await this.UploadImagesToCloudinaryAsync(Ethnic, EthnicName.ToLower().Replace(" ", "-"));
            var familyImage = await this.UploadImagesToCloudinaryAsync(FamilyStyle, FamilyStyleName.ToLower().Replace(" ", "-"));
            var fastFoodImage = await this.UploadImagesToCloudinaryAsync(FastFood, FastFoodName.ToLower().Replace(" ", "-"));
            var fineImage = await this.UploadImagesToCloudinaryAsync(FineDining, FineDiningName.ToLower().Replace(" ", "-"));
            var premiumImage = await this.UploadImagesToCloudinaryAsync(PremiumCasual, PremiumCasualName.ToLower().Replace(" ", "-"));
            var pubImage = await this.UploadImagesToCloudinaryAsync(Pub, PubName.ToLower().Replace(" ", "-"));

            List<(string ImageUrl, string PublicId)> categoryImages
                = new List<(string ImageUrl, string PublicId)>
            {
                (cafeteriaImage.ImageUrl, cafeteriaImage.PublicId),
                (casualImage.ImageUrl, casualImage.PublicId),
                (ethnicImage.ImageUrl, ethnicImage.PublicId),
                (familyImage.ImageUrl, familyImage.PublicId),
                (fastFoodImage.ImageUrl, fastFoodImage.PublicId),
                (fineImage.ImageUrl, fineImage.PublicId),
                (premiumImage.ImageUrl, premiumImage.PublicId),
                (pubImage.ImageUrl, pubImage.PublicId),
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

        private async Task<ImageBindingModel> UploadImagesToCloudinaryAsync(string imageUrl, string categoryName)
        {
            var image = await this.imageService.UploadCategoryImageToCloudinaryAsync(imageUrl, categoryName);

            return image;
        }
    }
}
