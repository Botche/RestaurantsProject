namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface IImageService
    {
        Task<string> UploadImageToCloudinaryAsync(string imageUrl);

        Task DeleteImageAsync(string imageUrl);
    }
}
