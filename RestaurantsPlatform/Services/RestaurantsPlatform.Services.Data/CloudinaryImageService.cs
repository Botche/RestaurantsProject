﻿namespace RestaurantsPlatform.Services.Data
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.Extensions.Configuration;

    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.RestaurantsImages;

    public class CloudinaryImageService : ICloudinaryImageService
    {
        private readonly Cloudinary cloudinary;
        private readonly IConfiguration configuration;

        public CloudinaryImageService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.cloudinary = this.InitializeCloudinary();
        }

        public async Task DeleteImageAsync(string publicId)
        {
            var deletionParams = new DeletionParams(publicId);
            await this.cloudinary.DestroyAsync(deletionParams);
        }

        public async Task<CategoryImageBindingModel> UploadCategoryImageToCloudinaryAsync(string imageUrl, string categoryName = null)
        {
            string path = categoryName == null ? "restaurant/categories"
                : $"restaurant/categories/{categoryName.ToLower().Replace(' ', '-')}";

            return await this.UploadImageToClodinary(imageUrl, path);
        }

        public async Task<CategoryImageBindingModel> UploadRestaurantImageToCloudinaryAsync(string imageUrl, string restaurantName = null)
        {
            string path = restaurantName == null ? "restaurant/restaurants"
                   : $"restaurant/restaurants/{restaurantName.ToLower().Replace(' ', '-')}";

            return await this.UploadImageToClodinary(imageUrl, path);
        }

        private async Task<CategoryImageBindingModel> UploadImageToClodinary(string imageUrl, string path)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageUrl),
                Folder = path,
                EagerAsync = true,
            };

            var uploadResult = await this.cloudinary.UploadAsync(uploadParams);

            var result = new CategoryImageBindingModel
            {
                ImageUrl = uploadResult.SecureUri.AbsoluteUri,
                PublicId = uploadResult.PublicId,
            };

            return result;
        }

        private Cloudinary InitializeCloudinary()
        {
            Account account = new Account(
                    this.configuration.GetSection("Cloudinary")["CloudName"],
                    this.configuration.GetSection("Cloudinary")["ApiKey"],
                    this.configuration.GetSection("Cloudinary")["ApiSecret"]);

            return new Cloudinary(account);
        }
    }
}
