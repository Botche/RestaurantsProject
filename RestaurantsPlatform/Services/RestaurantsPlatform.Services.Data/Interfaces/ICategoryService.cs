namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        IEnumerable<T> GetAllCategories<T>(int? count = null);

        T GetByIdAndName<T>(int id, string name);

        Task<int> CreateCategory(string description, string imageUrl, string name, string title);
    }
}
