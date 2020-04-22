// <copyright file="FavouritesSeeder.cs" company="RestaurantsPlatform">
// Copyright (c) RestaurantsPlatform. All Rights Reserved.
// </copyright>

namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Data.Common.Seeding.Restaurants.SeedInfo;

    /// <summary>
    /// Category seeder.
    /// </summary>
    public class FavouritesSeeder : ISeeder
    {
        /// <summary>
        /// Seeding method.
        /// </summary>
        /// <param name="dbContext">Database.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.FavouriteRestaurants.Any())
            {
                return;
            }

            var aladinFoods = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == AladinName);
            var aladinFoods2 = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .Where(restaurant => restaurant.RestaurantName == Aladin2Name)
                .ToList()[1];

            // Furna
            var furna = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == FurnaName);

            // Indian restaurant kohinoor
            var indian = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == IndianName);

            // Indian restaurant kohinoor
            var moma = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == MomaName);

            // Indian restaurant kohinoor
            var skapto = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == SkaptoName);

            // Indian restaurant kohinoor
            var wok = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == WokName);

            var admin = dbContext.Users.FirstOrDefault(user => user.Email == AdministratorEmail).Id;
            var user = dbContext.Users.FirstOrDefault(user => user.Email == UserEmail).Id;
            var owner = dbContext.Users.FirstOrDefault(user => user.Email == RestaurantEmail).Id;
            var secondOwner = dbContext.Users.FirstOrDefault(user => user.Email == SecondRestaurantEmail).Id;
            var realUser = dbContext.Users.FirstOrDefault(user => user.Email == RealUserEmailOne).Id;
            var realUserTwo = dbContext.Users.FirstOrDefault(user => user.Email == RealUserEmailTwo).Id;

            List<(int RestaurantId, string UserId)> favourites
                = new List<(int RestaurantId, string UserId)>
            {
                (aladinFoods.Id, user),
                (aladinFoods.Id, secondOwner),

                (aladinFoods2.Id, user),
                (aladinFoods2.Id, secondOwner),
                (aladinFoods2.Id, realUserTwo),

                (furna.Id, admin),
                (furna.Id, owner),
                (furna.Id, realUserTwo),

                (indian.Id, admin),
                (indian.Id, realUser),
                (indian.Id, realUserTwo),

                (moma.Id, admin),
                (moma.Id, owner),
                (moma.Id, realUserTwo),

                (skapto.Id, user),
                (skapto.Id, realUser),

                (wok.Id, user),
                (wok.Id, owner),
                (wok.Id, realUser),
            };

            foreach (var favourite in favourites)
            {
                await dbContext.FavouriteRestaurants.AddAsync(new FavouriteRestaurant
                {
                    RestaurantId = favourite.RestaurantId,
                    UserId = favourite.UserId,
                });
            }
        }
    }
}
