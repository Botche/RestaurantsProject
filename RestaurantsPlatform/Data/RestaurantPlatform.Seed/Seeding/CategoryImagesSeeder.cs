// <copyright file="CategoryImagesSeeder.cs" company="RestaurantsPlatform">
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
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    using static RestaurantsPlatform.Common.StringExtensions;
    using static RestaurantsPlatform.Data.Common.Seeding.Categories.SeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.CategoryImages.SeedInfo;

    /// <summary>
    /// Category seeder.
    /// </summary>
    public class CategoryImagesSeeder : ISeeder
    {
        private readonly ICloudinaryImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryImagesSeeder"/> class.
        /// </summary>
        /// <param name="imageService">Image service to upload images in cloud.</param>
        public CategoryImagesSeeder(ICloudinaryImageService imageService)
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
            if (dbContext.CategoryImages.Any())
            {
                return;
            }

            var cafeteriaImage = await this.UploadImagesToCloudinaryAsync(Cafeteria, CafeteriaName.ToSlug());
            var casualImage = await this.UploadImagesToCloudinaryAsync(CasualDining, CasualDiningName.ToSlug());
            var ethnicImage = await this.UploadImagesToCloudinaryAsync(Ethnic, EthnicName.ToSlug());
            var familyImage = await this.UploadImagesToCloudinaryAsync(FamilyStyle, FamilyStyleName.ToSlug());
            var fastFoodImage = await this.UploadImagesToCloudinaryAsync(FastFood, FastFoodName.ToSlug());
            var fineImage = await this.UploadImagesToCloudinaryAsync(FineDining, FineDiningName.ToSlug());
            var premiumImage = await this.UploadImagesToCloudinaryAsync(PremiumCasual, PremiumCasualName.ToSlug());
            var pubImage = await this.UploadImagesToCloudinaryAsync(Pub, PubName.ToSlug());

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
