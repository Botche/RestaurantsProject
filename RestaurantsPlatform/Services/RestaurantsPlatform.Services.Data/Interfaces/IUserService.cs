namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using RestaurantsPlatform.Data.Models;

    public interface IUserService
    {
        bool CheckIfCurrentUserIsAuthor(string authorId, string currentUserId);

        bool CheckIfCurrentUserIsNotAuthorByGivenId(int restaurantId, string currentUserId);

        IEnumerable<T> GetAllUsersWithDeleted<T>();

        ApplicationUser GetCurrentUserInfo(string userId);
    }
}
