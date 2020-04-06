namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

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

        public async Task DeleteImageAsync(string imageUrl)
        {
            var resourcesParams = new ListResourcesParams();
            var result = await this.cloudinary.ListResourcesAsync(resourcesParams);

            var publicId = string.Empty;
            foreach (var resource in result.Resources)
            {
                var resourcePath = resource.SecureUri.OriginalString;
                if (resourcePath == imageUrl)
                {
                    publicId = resource.PublicId;
                }
            }

            var deletionParams = new DeletionParams(publicId);
            await this.cloudinary.DestroyAsync(deletionParams);
        }

        public async Task<string> UploadImageToCloudinaryAsync(string imageUrl)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageUrl),
                Folder = "restaurant/categories",
                EagerAsync = true,
                EagerNotificationUrl = "https://localhost:44319/Categories/All",
            };
            var uploadResult = await this.cloudinary.UploadAsync(uploadParams);

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
