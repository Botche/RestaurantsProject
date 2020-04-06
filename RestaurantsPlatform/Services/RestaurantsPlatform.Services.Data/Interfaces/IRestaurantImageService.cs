namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface IRestaurantImageService
    {
        Task DeleteImageAsync(int imageId);

        Task<int> AddImageToRestaurantAsync(string imageUrl, string name, int restaurantId);
    }
}
