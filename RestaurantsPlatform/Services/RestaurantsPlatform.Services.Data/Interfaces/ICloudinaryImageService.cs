namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    public interface ICloudinaryImageService
    {
        Task<ImageBindingModel> UploadCategoryImageToCloudinaryAsync(string imageUrl, string categoryName = null);

        Task<ImageBindingModel> UploadRestaurantImageToCloudinaryAsync(string imageUrl, string restaurantName = null);

        Task DeleteImageAsync(string publicId);
    }
}
