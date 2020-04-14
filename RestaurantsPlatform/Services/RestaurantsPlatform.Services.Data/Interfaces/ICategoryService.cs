namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICategoryService
    {
        IEnumerable<T> GetAllCategories<T>();

        T GetByIdAndName<T>(int id, string name);

        Task<int> CreateCategoryAsync(string description, string imageUrl, string name, string title);

        T GetById<T>(int id);

        Task<int> UpdateCategoryAsync(int id, string description, string name, string title);

        Task<int> DeleteCategoryAsync(int id);

        Task<int> UpdateCategoryImageAsync(int categoryId, string imageUrl);

        IEnumerable<T> GetAllCategoriesWithPage<T>(int? take = null, int skip = 0);

        int GetCountOfAllCategories();
    }
}
