namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task<int?> AddCommentToRestaurantAsync(int id, string commentContent, string userId);

        Task<int?> DeleteCommentFromRestaurantAsync(int commentId, int restaurantId);
    }
}
