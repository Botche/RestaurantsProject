namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Models.Restaurants;

    public interface IRestaurantImageService
    {
        Task DeleteImageByIdAsync(int imageId);

        Task<int> AddImageToRestaurantAsync(string imageUrl, string name, int restaurantId);

        Task DeleteAllImagesAppenedToRestaurantAsync(int restaurantId);

        Task DeleteImageAsync(RestaurantImage image);
    }
}
