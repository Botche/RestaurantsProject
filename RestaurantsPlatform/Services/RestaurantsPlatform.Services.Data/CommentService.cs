namespace RestaurantsPlatform.Services.Data
{
    using System;
    using System.Threading.Tasks;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;

    public class CommentService : ICommentService
    {
        private readonly IDeletableEntityRepository<Comment> commentRepostitory;

        public CommentService(IDeletableEntityRepository<Comment> commentRepostitory)
        {
            this.commentRepostitory = commentRepostitory;
        }

        public async Task<int?> AddCommentToRestaurant(int id, string commentContent, string userId)
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
    }
}
