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

        public CommentService(IDeletableEntityRepository<Comment> commentRepostitory)
        {
            this.commentRepostitory = commentRepostitory;
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
