// <copyright file="CommentsSeeder.cs" company="RestaurantsPlatform">
// Copyright (c) RestaurantsPlatform. All Rights Reserved.
// </copyright>

namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Data.Common.Seeding.Comments.SeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.Restaurants.SeedInfo;

    /// <summary>
    /// Category seeder.
    /// </summary>
    public class CommentsSeeder : ISeeder
    {
        /// <summary>
        /// Seeding method.
        /// </summary>
        /// <param name="dbContext">Database.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Comments.AnyAsync())
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

            // Gurkha
            var gurkha = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == GurkhaName);

            // Memento
            var memento = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == MementoName);

            // Rainbow
            var rainbow = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == RainbowName);

            // Rosiche
            var rosiche = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == RosicheName);

            // Tenebris
            var tenebris = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == TenebrisName);

            // Cosmos
            var cosmos = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == CosmosName);

            // Bottega
            var bottega = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == BottegaName);

            // Sputnik
            var sputnik = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == SputnikName);

            // Cocktail
            var cocktail = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == CocktailName);

            // Gastro
            var gastro = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == GastroName);

            // Road
            var road = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == RoadName);

            // Oscar
            var oscar = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == OscarName);

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
                (furna.Id, realUserTwo, FurnaCommentContentThree),
                (furna.Id, realUser, FurnaCommentContentFour),
                (furna.Id, user, FurnaCommentContentFive),

                (indian.Id, admin, IndianCommentContent),
                (indian.Id, realUser, IndianCommentContentTwo),

                (moma.Id, admin, MomaCommentContent),
                (moma.Id, owner, MomaCommentContentTwo),
                (moma.Id, realUserTwo, MomaCommentContentThree),

                (skapto.Id, user, SkaptoCommentContent),

                (wok.Id, user, WokCommentContent),
                (wok.Id, owner, WokCommentContentTwo),

                (gurkha.Id, user, GurkhaCommentContent),
                (gurkha.Id, secodOwner, GurkhaCommentContentTwo),
                (gurkha.Id, owner, GurkhaCommentContentThree),

                (memento.Id, user, MementoCommentContent),
                (memento.Id, secodOwner, MementoCommentContentTwo),
                (memento.Id, owner, MementoCommentContentThree),
                (memento.Id, realUserTwo, MementoCommentContentFour),

                (rainbow.Id, user, RainbowCommentContent),
                (rainbow.Id, realUser, RainbowCommentContentTwo),
                (rainbow.Id, realUserTwo, RainbowCommentContentThree),

                (rosiche.Id, realUser, RosicheCommentContent),
                (rosiche.Id, realUserTwo, RosicheCommentContentTwo),

                (tenebris.Id, owner, TenebrisCommentContent),
                (tenebris.Id, secodOwner, TenebrisCommentContentTwo),

                (cosmos.Id, admin, CosmosCommentContent),
                (cosmos.Id, secodOwner, CosmosCommentContentTwo),
                (cosmos.Id, realUser, CosmosCommentContentBad),
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
