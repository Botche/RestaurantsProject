namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface IVoteService
    {
        Task VoteAsync(int commentId, string userId, bool isUpVote);

        int GetVotes(int commentId);

        Task DeleteAllVotesAppenedToCommentAsync(int commentId);
    }
}
