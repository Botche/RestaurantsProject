namespace RestaurantsPlatform.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;

    public class RestaurantImageService : IRestaurantImageService
    {
        private readonly IDeletableEntityRepository<RestaurantImage> imageRepository;
        private readonly ICloudinaryImageService cloudinaryImageService;

        public RestaurantImageService(
            IDeletableEntityRepository<RestaurantImage> restaurantImageRepository,
            ICloudinaryImageService cloudinaryImageService)
        {
            this.imageRepository = restaurantImageRepository;
            this.cloudinaryImageService = cloudinaryImageService;
        }

        public async Task DeleteImageByIdAsync(int imageId)
        {
            var image = this.GetImageById(imageId)
                .FirstOrDefault();

            await this.cloudinaryImageService.DeleteImageAsync(image.PublicId);

            this.imageRepository.Delete(image);
            await this.imageRepository.SaveChangesAsync();
        }

        public async Task DeleteImageAsync(RestaurantImage image)
        {
            await this.cloudinaryImageService.DeleteImageAsync(image.PublicId);

            this.imageRepository.Delete(image);
            await this.imageRepository.SaveChangesAsync();
        }

        public async Task DeleteAllImagesAppenedToRestaurantAsync(int restaurantId)
        {
            var images = this.GetImagesByRestaurantId(restaurantId)
                .ToList();

            foreach (var image in images)
            {
                await this.DeleteImageAsync(image);
            }

            await this.imageRepository.SaveChangesAsync();
        }

        public async Task<int> AddImageToRestaurantAsync(string imageUrl, string name, int restaurantId)
        {
            var result = await this.cloudinaryImageService.UploadRestaurantImageToCloudinaryAsync(imageUrl, name);

            var image = new RestaurantImage
            {
                ImageUrl = result.ImageUrl,
                PublicId = result.PublicId,
                RestaurantId = restaurantId,
            };

            await this.imageRepository.AddAsync(image);
            await this.imageRepository.SaveChangesAsync();

            return image.Id;
        }

        public async Task<int> UpdateRestaurantImageAsync(int id, string restaurantName, string imageUrl, string oldImageUrl)
        {
            var image = this.GetImagesByRestaurantId(id)
                .Where(image => image.ImageUrl == oldImageUrl)
                .FirstOrDefault();

            await this.DeleteImageAsync(image);
            return await this.AddImageToRestaurantAsync(imageUrl, restaurantName, id);
        }

        private IQueryable<RestaurantImage> GetImageById(int id)
        {
            return this.imageRepository.All()
                .Where(resturant => resturant.Id == id);
        }

        private IQueryable<RestaurantImage> GetImagesByRestaurantId(int restaurantId)
        {
            return this.imageRepository.All()
                .Where(resturant => resturant.RestaurantId == restaurantId);
        }
    }
}
