namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Models.Restaurants;

    public interface IRestaurantImageService
    {
        Task<int?> DeleteImageByIdAsync(int imageId);

        Task<int?> AddImageToRestaurantAsync(string imageUrl, string name, int restaurantId);

        Task<int?> DeleteAllImagesAppenedToRestaurantAsync(int restaurantId);

        Task<int?> DeleteImageAsync(RestaurantImage image);

        Task<int?> UpdateRestaurantImageAsync(int id, string restaurantName, string imageUrl, string oldImageUrl);
    }
}
