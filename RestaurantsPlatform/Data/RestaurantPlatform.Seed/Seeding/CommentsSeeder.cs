﻿namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Data.Common.Seeding.Comments.SeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.Restaurants.SeedInfo;

    public class CommentsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Comments.Any())
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
            var secodOwner = dbContext.Users.FirstOrDefault(user => user.Email == SecondRestaurantEmail).Id;
            var realUser = dbContext.Users.FirstOrDefault(user => user.Email == RealUserEmailOne).Id;
            var realUserTwo = dbContext.Users.FirstOrDefault(user => user.Email == RealUserEmailTwo).Id;

            List<(int RestaurantId, string UserId, string Content)> comments
                = new List<(int RestaurantId, string UserId, string Content)>
            {
                (furna.Id, admin, FurnaCommentContent),
                (furna.Id, owner, FurnaCommentContentTwo),

                (indian.Id, admin, IndianCommentContent),
                (indian.Id, realUser, IndianCommentContentTwo),

                (moma.Id, admin, MomaCommentContent),
                (moma.Id, owner, MomaCommentContentTwo),
                (moma.Id, realUserTwo, MomaCommentContentThree),

                (skapto.Id, user, SkaptoCommentContent),

                (wok.Id, user, WokCommentContent),
                (wok.Id, owner, WokCommentContentTwo),
            };

            foreach (var comment in comments)
            {
                await dbContext.Comments.AddAsync(new Comment
                {
                    RestaurantId = comment.RestaurantId,
                    AuthorId = comment.UserId,
                    Content = comment.Content,
                });
            }
        }
    }
}