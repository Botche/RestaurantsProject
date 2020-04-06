namespace RestaurantsPlatform.Services.Data.Interfaces
{
    public interface IUserService
    {
        bool CheckIfCurrentUserIsAuthor(string authorId, string currentUserId);

        bool CheckIfCurrentUserIsAuthorByGivenId(int restaurantId, string currentUserId);
    }
}
