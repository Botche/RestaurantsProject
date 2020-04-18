namespace RestaurantsPlatform.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;

    public class FavouriteService : IFavouriteService
    {
        private readonly IRepository<FavouriteRestaurant> favoriteRepository;

        public FavouriteService(IRepository<FavouriteRestaurant> favoriteRepository)
        {
            this.favoriteRepository = favoriteRepository;
        }

        public bool CheckIfRestaurantIsFavourite(int id, string userId)
        {
            return this.favoriteRepository.All()
                .Any(favourite => favourite.RestaurantId == id && favourite.UserId == userId);
        }

        public async Task<string> FavouriteAsync(int restaurantId, string userId, bool isFavourite)
        {
            if (isFavourite)
            {
                return await this.CreateFavouriteAsync(restaurantId, userId);
            }
            else
            {
                return await this.DeleteFavouriteAsync(restaurantId, userId);
            }
        }

        private async Task<string> CreateFavouriteAsync(int restaurantId, string userId)
        {
            var favourite = new FavouriteRestaurant()
            {
                RestaurantId = restaurantId,
                UserId = userId,
            };

            await this.favoriteRepository.AddAsync(favourite);
            await this.favoriteRepository.SaveChangesAsync();

            return favourite.Id;
        }

        private async Task<string> DeleteFavouriteAsync(int restaurantId, string userId)
        {
            var favourite = this.favoriteRepository.All()
                                .Where(favourite => favourite.UserId == userId && favourite.RestaurantId == restaurantId)
                                .FirstOrDefault();

            if (favourite == null)
            {
                return null;
            }

            this.favoriteRepository.Delete(favourite);
            await this.favoriteRepository.SaveChangesAsync();

            return favourite.Id;
        }
    }
}
