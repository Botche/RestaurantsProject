namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface IFavouriteService
    {
        Task<string> FavouriteAsync(int restaurantId, string userId, bool isFavourite);

        bool CheckIfRestaurantIsFavourite(int id, string userId);
    }
}
