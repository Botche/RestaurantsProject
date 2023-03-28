namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface ICategoryImageService
    {
        Task<int> AddImageToCategoryAsync(string imageUrl, string categoryName);

        Task DeleteImageAsync(int imageId);
    }
}
