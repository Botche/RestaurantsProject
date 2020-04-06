namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.Extensions.Configuration;

    public class ImageService : IImageService
    {
        private readonly Cloudinary cloudinary;
        private readonly IConfiguration configuration;

        public ImageService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.cloudinary = this.InitializeCloudinary();
        }

        public string UploadImageToCloudinary(string imageUrl)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageUrl),
                Folder = "restaurant/categories",
                EagerAsync = true,
                EagerNotificationUrl = "https://localhost:44319/Categories/All",
            };
            var uploadResult = this.cloudinary.Upload(uploadParams);

            return uploadResult.SecureUri.AbsoluteUri;
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
