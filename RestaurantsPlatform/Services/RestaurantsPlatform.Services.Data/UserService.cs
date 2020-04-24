namespace RestaurantsPlatform.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;
    using RestaurantsPlatform.Web.ViewModels.UserImage;

    public class UserService : IUserService
    {
        private readonly IRestaurantService restaurantService;
        private readonly IUserImageSercice imageService;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UserService(
            IRestaurantService restaurantService,
            IUserImageSercice imageService,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.restaurantService = restaurantService;
            this.imageService = imageService;
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

        public async Task<string> DeleteProfilePictureByUsernameAsync(string userName)
        {
            var userImageUrl = this.usersRepository.All()
                .Select(user => new
                {
                    user.UserName,
                    user.ImageUrl,
                })
                .FirstOrDefault(user => user.UserName == userName)
                .ImageUrl;

            if (userImageUrl == null)
            {
                return null;
            }

            var imageUrl = await this.imageService.DeleteImageFromUserAsync(userName);
            return imageUrl;
        }

        public async Task<string> UpdateProfilePictureByAsync(string userName, string imageUrl)
        {
            var user = this.usersRepository.All()
                .Select(user => new
                {
                    user.UserName,
                })
                .FirstOrDefault(user => user.UserName == userName);

            if (user == null)
            {
                return null;
            }

            var newImageUrl = await this.imageService.AddImageToUserAsync(userName, imageUrl);
            return newImageUrl;
        }

        public IEnumerable<T> GetAllUsersWithDeleted<T>()
        {
            return this.usersRepository.AllWithDeleted().To<T>().ToList();
        }

        public T GetUserInfoByUsername<T>(string username)
        {
            return this.usersRepository.All()
                .Where(user => user.UserName == username)
                .To<T>()
                .FirstOrDefault();
        }

        public UserImageInputModel GetUserImage(string userName)
        {
            var image = this.usersRepository.All()
                .Where(user => user.UserName == userName)
                .To<UserImageInputModel>()
                .FirstOrDefault();

            return image;
        }
    }
}
