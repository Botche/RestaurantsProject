namespace RestaurantsPlatform.Services.Data.Interfaces
{
    public interface IUserService
    {
        bool CheckIfCurrentUserIsAuthor(string authorId, string currentUserId);

        bool CheckIfCurrentUserIsNotAuthorByGivenId(int restaurantId, string currentUserId);
    }
}
