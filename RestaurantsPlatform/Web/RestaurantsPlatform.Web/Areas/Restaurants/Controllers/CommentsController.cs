namespace RestaurantsPlatform.Web.Areas.Restaurants.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Comments;

    using static RestaurantsPlatform.Web.Infrastructure.ErrorConstants;
    using static RestaurantsPlatform.Web.Infrastructure.NotificationsMessagesContants;

    [Area("Restaurants")]
    [Authorize]
    public class CommentsController : BaseController
    {
        private readonly ICommentService commentService;

        public CommentsController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        public async Task<IActionResult> AddCommentToRestaurant(CreateCommentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName.ToLower().Replace(' ', '-') });
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var id = await this.commentService.AddCommentToRestaurant(input.Id, input.CommentContent, userId);

            if (id == 0)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                });
            }

            this.TempData[SuccessNotification] = string.Format(SuccessfullyCommentRestaurant, input.RestaurantName);
            return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName.ToLower().Replace(" ", "-") });
        }
    }
}