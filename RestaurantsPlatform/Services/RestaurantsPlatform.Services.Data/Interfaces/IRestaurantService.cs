namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRestaurantService
    {
        IEnumerable<T> GetByCategoryId<T>(int categoryId, int? take = null, int skip = 0);

        int GetCountByCategoryId(int id);

        Task CreateRestaurant(string address, int categoryId, string contactInfo, string description, string ownerName, string restaurantName, string workingTime);
    }
}
