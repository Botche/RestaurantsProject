namespace RestaurantsPlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;

    public class RestaurantService : IRestaurantService
    {
        private readonly IDeletableEntityRepository<Restaurant> restaurantRespository;

        public RestaurantService(IDeletableEntityRepository<Restaurant> restaurantRespository)
        {
            this.restaurantRespository = restaurantRespository;
        }

        public async Task CreateRestaurant(string address, int categoryId, string contactInfo, string description, string ownerName, string restaurantName, string workingTime)
        {
            Restaurant restaurant = new Restaurant
            {
                Address = address,
                CategoryId = categoryId,
                ContactInfo = contactInfo,
                CreatedOn = DateTime.UtcNow,
                Description = description,
                IsDeleted = false,
                OwnerName = ownerName,
                RestaurantName = restaurantName,
                WorkingTime = workingTime,
            };

            await this.restaurantRespository.AddAsync(restaurant);
            await this.restaurantRespository.SaveChangesAsync();
        }

        public IEnumerable<T> GetByCategoryId<T>(int categoryId, int? take = null, int skip = 0)
        {
            IQueryable<Restaurant> restaurants = this.restaurantRespository.All()
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
            return this.restaurantRespository.All()
                .Where(restaurant => restaurant.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public int GetCountByCategoryId(int id)
        {
            return this.restaurantRespository.All().Count();
        }
    }
}
