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

    public class CommentService : ICommentService
    {
        private readonly IDeletableEntityRepository<Comment> commentRepostitory;
        private readonly IVoteService voteService;

        public CommentService(
            IDeletableEntityRepository<Comment> commentRepostitory,
            IVoteService voteService)
        {
            this.commentRepostitory = commentRepostitory;
            this.voteService = voteService;
        }

        public async Task<int?> AddCommentToRestaurantAsync(int id, string commentContent, string userId)
        {
            var comment = new Comment()
            {
                Content = commentContent,
                RestaurantId = id,
                AuthorId = userId,
            };

            await this.commentRepostitory.AddAsync(comment);
            await this.commentRepostitory.SaveChangesAsync();

            return comment.Id;
        }

        public async Task DeleteAllCommentsAppendedToRestaurantAsync(int id)
        {
            var commentsToDelete = this.commentRepostitory.All()
                .Where(comment => comment.RestaurantId == id)
                .Select(comment => comment.Id)
                .ToList();

            foreach (var commentToDelete in commentsToDelete)
            {
                await this.DeleteCommentFromRestaurantAsync(commentToDelete, id);
            }
        }

        public async Task<int?> DeleteCommentFromRestaurantAsync(int commentId, int restaurantId)
        {
            var comment = this.commentRepostitory.All()
                .Where(comment => comment.Id == commentId && comment.RestaurantId == restaurantId)
                .FirstOrDefault();

            if (comment == null)
            {
                return null;
            }

            this.commentRepostitory.Delete(comment);
            await this.commentRepostitory.SaveChangesAsync();
            await this.voteService.DeleteAllVotesAppenedToCommentAsync(comment.Id);

            return comment.Id;
        }

        public IEnumerable<T> GetLatestComments<T>(int restaurantId)
        {
            return this.commentRepostitory.All()
                .Where(comment => comment.RestaurantId == restaurantId)
                .OrderByDescending(comment => comment.CreatedOn)
                .To<T>()
                .ToList();
        }

        public IEnumerable<T> GetMostPopularComments<T>(int restaurantId)
        {
            return this.commentRepostitory.All()
                .Where(comment => comment.RestaurantId == restaurantId)
                .OrderByDescending(comment => comment.Votes.Sum(vote => (int)vote.Type))
                .To<T>()
                .ToList();
        }

        public Task<int> UpdateCommentAsync(int commentId, string content)
        {
            var commentToUpdate = this.commentRepostitory.All()
                .Where(comment => comment.Id == commentId)
                .FirstOrDefault();

            commentToUpdate.Content = content;

            this.commentRepostitory.Update(commentToUpdate);
            return this.commentRepostitory.SaveChangesAsync();
        }
    }
}
