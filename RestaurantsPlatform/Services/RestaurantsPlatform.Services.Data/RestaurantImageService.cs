namespace RestaurantsPlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.RestaurantsImages;

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

        public async Task<int?> DeleteImageAsync(RestaurantImage image)
        {
            if (image == null)
            {
                return null;
            }

            await this.cloudinaryImageService.DeleteImageAsync(image.PublicId);

            this.imageRepository.Delete(image);
            return await this.imageRepository.SaveChangesAsync();
        }

        public async Task<int?> DeleteAllImagesAppenedToRestaurantAsync(int restaurantId)
        {
            var images = await this.GetImagesByRestaurantId(restaurantId)
                .ToListAsync();

            foreach (var image in images)
            {
                await this.DeleteImageAsync(image);
            }

            return await this.imageRepository.SaveChangesAsync();
        }

        public async Task<int?> AddImageToRestaurantAsync(string imageUrl, string name, int restaurantId)
        {
            var result = await this.cloudinaryImageService.UploadRestaurantImageToCloudinaryAsync(imageUrl, name);

            if (result == null)
            {
                return null;
            }

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

        public async Task<int?> UpdateRestaurantImageAsync(int id, string restaurantName, string imageUrl, string oldImageUrl)
        {
            var image = await this.GetImagesByRestaurantId(id)
                .Where(image => image.ImageUrl == oldImageUrl)
                .FirstOrDefaultAsync();

            if (image == null)
            {
                return null;
            }

            await this.DeleteImageAsync(image);
            return await this.AddImageToRestaurantAsync(imageUrl, restaurantName, id);
        }

        public async Task<IList<DisplayImageOnFrontPageViewModel>> GetRandomImagesForIndexPageAsync(int count)
        {
            var images = new List<DisplayImageOnFrontPageViewModel>();

            for (int index = 0; index < count; index++)
            {
                DisplayImageOnFrontPageViewModel randomImage = null;
                do
                {
                    var random = new Random();

                    var randomRestaurantImageId = random.Next(
                        this.imageRepository.All().OrderBy(image => image.Id).First().Id,
                        this.imageRepository.All().OrderByDescending(image => image.Id).First().Id);

                    randomImage = await this.imageRepository.All()
                        .Where(image => image.Id == randomRestaurantImageId)
                        .To<DisplayImageOnFrontPageViewModel>()
                        .FirstOrDefaultAsync();
                }
                while (randomImage == null || images.Any(image => image.Id == randomImage.Id));

                images.Add(randomImage);
            }

            return images;
        }

        private IQueryable<RestaurantImage> GetImagesByRestaurantId(int restaurantId)
        {
            return this.imageRepository.All()
                .Where(resturant => resturant.RestaurantId == restaurantId);
        }
    }
}
