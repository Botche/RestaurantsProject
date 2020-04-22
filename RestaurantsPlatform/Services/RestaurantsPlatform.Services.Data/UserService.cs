namespace RestaurantsPlatform.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    public class UserService : IUserService
    {
        private readonly IRestaurantService restaurantService;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UserService(
            IRestaurantService restaurantService,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.restaurantService = restaurantService;
            this.usersRepository = usersRepository;
        }

        public bool CheckIfCurrentUserIsAuthor(string authorId, string currentUserId)
        {
            return authorId != currentUserId;
        }

        public bool CheckIfCurrentUserIsNotAuthorByGivenId(int restaurantId, string currentUserId)
        {
            string authorId = this.restaurantService.GetById<AuthorIdFromRestaurantBindingModel>(restaurantId)?.UserId;

            return authorId != currentUserId;
        }

        public IEnumerable<T> GetAllUsersWithDeleted<T>()
        {
            return this.usersRepository.AllWithDeleted().To<T>().ToList();
        }

        public T GetCurrentUserInfo<T>(string userId)
        {
            return this.usersRepository.All()
                .Where(user => user.Id == userId)
                .To<T>()
                .FirstOrDefault();
        }
    }
}
