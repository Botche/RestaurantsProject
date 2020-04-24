namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface IUserService
    {
        bool CheckIfCurrentUserIsAuthor(string authorId, string currentUserId);

        bool CheckIfCurrentUserIsNotAuthorByGivenId(int restaurantId, string currentUserId);

        IEnumerable<T> GetAllUsersWithDeleted<T>();

        T GetUserInfoByUsername<T>(string username);
    }
}
