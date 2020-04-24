namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Models;

    public interface IUserImageSercice
    {
        Task<string> AddDefaultImageToUserAsync(ApplicationUser user);

        Task<string> AddImageToUserAsync(string username, string imageUrl);

        Task<string> DeleteImageFromUserAsync(string username);
    }
}
