namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;

    public interface ICategoryService
    {
        IEnumerable<T> GetAllCategories<T>(int? count = null);

        T GetByIdAndName<T>(int id, string name);
    }
}
