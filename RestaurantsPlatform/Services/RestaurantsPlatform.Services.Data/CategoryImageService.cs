namespace RestaurantsPlatform.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;

    public class CategoryImageService : ICategoryImageService
    {
        private readonly IDeletableEntityRepository<CategoryImage> imageRepository;
        private readonly ICloudinaryImageService cloudinaryImageService;

        public CategoryImageService(
            IDeletableEntityRepository<CategoryImage> imageRepository,
            ICloudinaryImageService cloudinaryImageService)
        {
            this.imageRepository = imageRepository;
            this.cloudinaryImageService = cloudinaryImageService;
        }

        public async Task DeleteImageAsync(int imageId)
        {
            var image = this.GetImageById(imageId)
                .FirstOrDefault();

            await this.cloudinaryImageService.DeleteImageAsync(image.PublicId);

            this.imageRepository.Delete(image);
            await this.imageRepository.SaveChangesAsync();
        }

        public async Task<int> AddImageToCategoryAsync(string imageUrl, string categoryName)
        {
            var result = await this.cloudinaryImageService.UploadCategoryImageToCloudinaryAsync(imageUrl, categoryName);

            var image = new CategoryImage
            {
                ImageUrl = result.ImageUrl,
                PublicId = result.PublicId,
            };

            await this.imageRepository.AddAsync(image);
            await this.imageRepository.SaveChangesAsync();

            return image.Id;
        }

        private IQueryable<CategoryImage> GetImageById(int id)
        {
            return this.imageRepository
                .All()
                .Where(image => image.Id == id);
        }
    }
}
