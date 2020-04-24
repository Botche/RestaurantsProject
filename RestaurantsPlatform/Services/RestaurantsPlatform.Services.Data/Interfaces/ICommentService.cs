namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task<int?> AddCommentToRestaurantAsync(int id, string commentContent, string userId);

        Task<int?> DeleteCommentFromRestaurantAsync(int commentId, int restaurantId);

        Task<int> UpdateCommentAsync(int commentId, string content);

        IEnumerable<T> GetLatestComments<T>(int restaurantId);

        IEnumerable<T> GetMostPopularComments<T>(int restaurantId);

        Task DeleteAllCommentsAppendedToRestaurantAsync(int id);
    }
}
