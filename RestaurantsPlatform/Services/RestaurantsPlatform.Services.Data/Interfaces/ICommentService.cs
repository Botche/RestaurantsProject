namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task<int?> AddCommentToRestaurant(int id, string commentContent, string userId);
    }
}
