namespace RestaurantsPlatform.Services.Data
{
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    public class UserService : IUserService
    {
        private readonly IRestaurantService restaurantService;

        public UserService(
            IRestaurantService restaurantService)
        {
            this.restaurantService = restaurantService;
        }

        public bool CheckIfCurrentUserIsAuthor(string authorId, string currentUserId)
        {
            return authorId != currentUserId;
        }

        public bool CheckIfCurrentUserIsNotAuthorByGivenId(int restaurantId, string currentUserId)
        {
            string authorId = this.restaurantService.GetById<AuthorIdFromRestaurantBindingModel>(restaurantId).UserId;

            return authorId != currentUserId;
        }
    }
}
