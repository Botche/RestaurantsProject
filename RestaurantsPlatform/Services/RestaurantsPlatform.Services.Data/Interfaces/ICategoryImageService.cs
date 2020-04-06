namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface ICategoryImageService
    {
        Task<int> AddImageToCategoryAsync(string imageUrl, string name);

        Task DeleteImageAsync(int imageId);
    }
}
