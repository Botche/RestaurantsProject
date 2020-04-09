﻿namespace RestaurantsPlatform.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;

    public class RestaurantService : IRestaurantService
    {
        private readonly IDeletableEntityRepository<Restaurant> restaurantRespository;
        private readonly IRestaurantImageService imageService;

        public RestaurantService(
            IDeletableEntityRepository<Restaurant> restaurantRespository,
            IRestaurantImageService imageService)
        {
            this.restaurantRespository = restaurantRespository;
            this.imageService = imageService;
        }

        public async Task<int> CreateRestaurantAsync(string userId, string address, int categoryId, string contactInfo, string description, string ownerName, string restaurantName, string workingTime)
        {
            Restaurant restaurant = new Restaurant
            {
                Address = address,
                CategoryId = categoryId,
                ContactInfo = contactInfo,
                Description = description,
                OwnerName = ownerName,
                RestaurantName = restaurantName,
                UserId = userId,
                WorkingTime = workingTime,
            };

            await this.restaurantRespository.AddAsync(restaurant);
            await this.restaurantRespository.SaveChangesAsync();

            return restaurant.Id;
        }

        public async Task<int> DeleteRestaurantByIdAsync(int id)
        {
            var restaurant = this.restaurantRespository.All()
                .Where(restaurant => restaurant.Id == id)
                .FirstOrDefault();

            this.restaurantRespository.Delete(restaurant);
            await this.restaurantRespository.SaveChangesAsync();

            await this.imageService.DeleteAllImagesAppenedToRestaurantAsync(restaurant.Id);

            return restaurant.Id;
        }

        public async Task DeleteAllRestaurantsAppenedToCategoryAsync(int categoryId)
        {
            var restaurants = this.restaurantRespository.All()
                .Where(restaurant => restaurant.CategoryId == categoryId)
                .ToList();

            foreach (var restaurant in restaurants)
            {
                await this.DeleteRestaurantByIdAsync(restaurant.Id);
            }

            await this.restaurantRespository.SaveChangesAsync();
        }

        public async Task<int> UpdateRestaurantAsync(int id, string ownerName, string restaurantName, string workingTime, string address, string contactInfo, string description, int categoryId)
        {
            Restaurant oldEntity = this.GetRestaurantById(id)
                .FirstOrDefault();

            oldEntity.OwnerName = ownerName;
            oldEntity.RestaurantName = restaurantName;
            oldEntity.WorkingTime = workingTime;
            oldEntity.Address = address;
            oldEntity.ContactInfo = contactInfo;
            oldEntity.Description = description;
            oldEntity.CategoryId = categoryId;

            this.restaurantRespository.Update(oldEntity);
            await this.restaurantRespository.SaveChangesAsync();

            return oldEntity.Id;
        }

        public IEnumerable<T> GetByCategoryId<T>(int categoryId, int? take = null, int skip = 0)
        {
            IQueryable<Restaurant> restaurants = this.restaurantRespository.All()
                .Where(restaurant => restaurant.CategoryId == categoryId)
                .OrderBy(restaurant => restaurant.RestaurantName)
                .Skip(skip);

            if (take.HasValue)
            {
                restaurants.Take(take.Value);
            }

            return restaurants.To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            var restaurant = this.GetRestaurantById(id)
                .Include(restaurant => restaurant.Images)
                .To<T>()
                .FirstOrDefault();

            return restaurant;
        }

        public T GetByIdAndName<T>(int id, string name)
        {
            string nameWithoutDashes = name.Replace('-', ' ');

            return this.GetRestaurantById(id)
                .Where(restaurant => restaurant.RestaurantName.ToLower() == nameWithoutDashes.ToLower())
                .To<T>()
                .FirstOrDefault();
        }

        public int GetCountByCategoryId(int id)
        {
            return this.restaurantRespository.All().Count();
        }

        public async Task DeleteImageByRestaurantIdAsync(int id, string imageUrl)
        {
            var image = this.GetRestaurantById(id)
                .Include(restaurant => restaurant.Images)
                .Select(restaurant => restaurant.Images.FirstOrDefault(image => image.ImageUrl == imageUrl))
                .FirstOrDefault();

            await this.imageService.DeleteImageAsync(image);
        }

        private IQueryable<Restaurant> GetRestaurantById(int id)
        {
            var restaurant = this.restaurantRespository.All()
                .Where(resturant => resturant.Id == id);

            return restaurant;
        }
    }
}
