namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    using RestaurantsPlatform.Web.ViewModels.Images;

    public interface ICloudinaryImageService
    {
        Task<CategoryImageBindingModel> UploadCategoryImageToCloudinaryAsync(string imageUrl, string categoryName = null);

        Task<CategoryImageBindingModel> UploadRestaurantImageToCloudinaryAsync(string imageUrl, string restaurantName = null);

        Task DeleteImageAsync(string publicId);
    }
}
