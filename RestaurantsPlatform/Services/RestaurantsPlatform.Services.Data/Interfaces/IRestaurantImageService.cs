﻿namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Web.ViewModels.RestaurantsImages;

    public interface IRestaurantImageService
    {
        Task<int?> AddImageToRestaurantAsync(string imageUrl, string name, int restaurantId);

        Task<int?> DeleteAllImagesAppenedToRestaurantAsync(int restaurantId);

        Task<int?> DeleteImageAsync(RestaurantImage image);

        Task<int?> UpdateRestaurantImageAsync(int id, string restaurantName, string imageUrl, string oldImageUrl);

        Task<IList<DisplayImageOnFrontPageViewModel>> GetRandomImagesForIndexPageAsync(int count);
    }
}
