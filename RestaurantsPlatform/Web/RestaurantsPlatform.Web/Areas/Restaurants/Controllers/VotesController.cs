namespace RestaurantsPlatform.Web.Areas.Restaurants.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Votes;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService votesService;

        public VotesController(
            IVoteService votesService)
        {
            this.votesService = votesService;
        }

        [HttpPost]
        public async Task<ActionResult<VoteResponseModel>> Post(VoteInputModel input)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var commentId = int.Parse(input.CommentId);
            await this.votesService.VoteAsync(commentId, userId, input.IsUpVote);
            var votes = this.votesService.GetVotes(commentId);

            return new VoteResponseModel { VotesCount = votes };
        }
    }
}
