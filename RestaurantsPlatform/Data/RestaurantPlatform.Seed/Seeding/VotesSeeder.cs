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

            var firstCommentId = dbContext.Comments.ToList()[0].Id;
            var secondCommentId = dbContext.Comments.ToList()[1].Id;
            var thirdCommentId = dbContext.Comments.ToList()[2].Id;
            var fourthCommentId = dbContext.Comments.ToList()[3].Id;
            var fifthCommentId = dbContext.Comments.ToList()[4].Id;
            var sixthCommentId = dbContext.Comments.ToList()[5].Id;
            var seventhCommentId = dbContext.Comments.ToList()[6].Id;
            var eighthCommentId = dbContext.Comments.ToList()[7].Id;
            var ninthCommentId = dbContext.Comments.ToList()[8].Id;
            var tenthCommentId = dbContext.Comments.ToList()[9].Id;

            var admin = dbContext.Users.FirstOrDefault(user => user.Email == AdministratorEmail).Id;
            var user = dbContext.Users.FirstOrDefault(user => user.Email == UserEmail).Id;
            var owner = dbContext.Users.FirstOrDefault(user => user.Email == RestaurantEmail).Id;
            var secondOwner = dbContext.Users.FirstOrDefault(user => user.Email == SecondRestaurantEmail).Id;
            var realUser = dbContext.Users.FirstOrDefault(user => user.Email == RealUserEmailOne).Id;
            var realUserTwo = dbContext.Users.FirstOrDefault(user => user.Email == RealUserEmailTwo).Id;

            List<(int CommentId, string UserId, VoteType Type)> comments
                = new List<(int CommentId, string UserId, VoteType Type)>
            {
                (firstCommentId, admin, VoteType.UpVote),
                (firstCommentId, user, VoteType.UpVote),
                (firstCommentId, owner, VoteType.UpVote),
                (firstCommentId, secondOwner, VoteType.UpVote),
                (firstCommentId, realUser, VoteType.UpVote),
                (firstCommentId, realUserTwo, VoteType.UpVote),

                (secondCommentId, owner, VoteType.UpVote),
                (secondCommentId, realUser, VoteType.UpVote),

                (thirdCommentId, admin, VoteType.UpVote),
                (thirdCommentId, realUserTwo, VoteType.UpVote),

                (fourthCommentId, secondOwner, VoteType.UpVote),

                (fifthCommentId, admin, VoteType.UpVote),
                (fifthCommentId, user, VoteType.UpVote),
                (fifthCommentId, owner, VoteType.DownVote),

                (seventhCommentId, realUserTwo, VoteType.UpVote),

                (eighthCommentId, user, VoteType.UpVote),
                (eighthCommentId, secondOwner, VoteType.UpVote),

                (ninthCommentId, admin, VoteType.UpVote),
            };

            foreach (var comment in comments)
            {
                await dbContext.Votes.AddAsync(new Vote
                {
                    Type = comment.Type,
                    UserId = comment.UserId,
                    CommentId = comment.CommentId,
                });
            }
        }
    }
}
