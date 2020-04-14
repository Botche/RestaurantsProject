﻿namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRestaurantService
    {
        IEnumerable<T> GetRestaurantsByCategoryId<T>(int categoryId, int? take = null, int skip = 0);

        int GetCountByCategoryId(int id);

        Task<int> CreateRestaurantAsync(string userId, string address, int categoryId, string contactInfo, string description, string ownerName, string restaurantName, string workingTime);

        T GetByIdAndName<T>(int id, string name);

        T GetById<T>(int id);

        Task<int?> UpdateRestaurantAsync(int id, string ownerName, string restaurantName, string workingTime, string address, string contactInfo, string description, int categoryId);

        Task<int?> DeleteRestaurantByIdAsync(int id);

        Task<int> DeleteAllRestaurantsAppenedToCategoryAsync(int categoryId);

        Task<int?> DeleteImageByRestaurantIdAsync(int id, string imageUrl);

        T GetRestaurantByIdWithImage<T>(int id, string imageUrl);
    }
}
