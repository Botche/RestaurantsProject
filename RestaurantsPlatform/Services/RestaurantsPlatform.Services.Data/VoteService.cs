namespace RestaurantsPlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;

    public class VoteService : IVoteService
    {
        private readonly IRepository<Vote> votesRepository;

        public VoteService(IRepository<Vote> votesRepository)
        {
            this.votesRepository = votesRepository;
        }

        public int GetVotes(int commentId)
        {
            var votes = this.votesRepository.All()
                .Where(x => x.CommentId == commentId).Sum(x => (int)x.Type);

            return votes;
        }

        public async Task VoteAsync(int commentId, string userId, bool isUpVote)
        {
            var vote = this.votesRepository.All()
                .FirstOrDefault(x => x.CommentId == commentId && x.UserId == userId);
            if (vote != null)
            {
                if (vote.Type == VoteType.UpVote)
                {
                    vote.Type = isUpVote ? VoteType.Neutral : VoteType.DownVote;
                }
                else if (vote.Type == VoteType.DownVote)
                {
                    vote.Type = isUpVote ? VoteType.UpVote : VoteType.Neutral;
                }
                else
                {
                    vote.Type = isUpVote ? VoteType.UpVote : VoteType.DownVote;
                }
            }
            else
            {
                vote = new Vote
                {
                    CommentId = commentId,
                    UserId = userId,
                    Type = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                };

                await this.votesRepository.AddAsync(vote);
            }

            await this.votesRepository.SaveChangesAsync();
        }
    }
}
