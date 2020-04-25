// <copyright file="VotesSeeder.cs" company="RestaurantsPlatform">
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

    /// <summary>
    /// Seeder for votes.
    /// </summary>
    public class VotesSeeder : ISeeder
    {
        /// <summary>
        /// Method to seed votes to database.
        /// </summary>
        /// <param name="dbContext">Database to seed in.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Votes.Any())
            {
                return;
            }

            var comments = dbContext.Comments
                .Select(comment => comment.Id)
                .ToList();

            var users = dbContext.Users
                .Select(user => user.Id)
                .ToList();

            var random = new Random();
            foreach (var commentId in comments)
            {
                foreach (var userId in users)
                {
                    var type = random.Next(-1, 2);

                    await dbContext.Votes.AddAsync(new Vote
                    {
                        Type = (VoteType)type,
                        UserId = userId,
                        CommentId = commentId,
                    });
                }
            }
        }
    }
}
