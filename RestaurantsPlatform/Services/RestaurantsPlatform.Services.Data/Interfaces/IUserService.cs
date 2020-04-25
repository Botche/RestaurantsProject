namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Web.ViewModels.CategoryImages;
    using RestaurantsPlatform.Web.ViewModels.UserImage;

    public interface IUserService
    {
        bool CheckIfCurrentUserIsAuthor(string authorId, string currentUserId);

        bool CheckIfCurrentUserIsNotAuthorByGivenId(int restaurantId, string currentUserId);

        IEnumerable<T> GetAllUsersWithDeleted<T>();

        T GetUserInfoByUsername<T>(string username);

        Task<string> DeleteProfilePictureByUsernameAsync(string userName);

        Task<string> UpdateProfilePictureByAsync(string userName, string imageUrl);

        UserImageInputModel GetUserImage(string userName);
    }
}
