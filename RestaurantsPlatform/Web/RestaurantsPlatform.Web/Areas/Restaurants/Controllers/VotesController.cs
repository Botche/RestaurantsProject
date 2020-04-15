namespace RestaurantsPlatform.Web.Areas.Restaurants.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Votes;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService votesService;
        private readonly UserManager<ApplicationUser> userManager;

        public VotesController(
            IVoteService votesService,
            UserManager<ApplicationUser> userManager)
        {
            this.votesService = votesService;
            this.userManager = userManager;
        }

        // POST /api/votes
        // Request body: {"postId":1,"isUpVote":true}
        // Response body: {"votesCount":16}
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<VoteResponseModel>> Post(VoteInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            var commentId = int.Parse(input.CommentId);
            await this.votesService.VoteAsync(commentId, userId, input.IsUpVote);
            var votes = this.votesService.GetVotes(commentId);

            return new VoteResponseModel { VotesCount = votes };
        }
    }
}