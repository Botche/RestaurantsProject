namespace RestaurantsPlatform.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels;

    using static RestaurantsPlatform.Web.Infrastructure.ErrorConstants;

    public class BaseController : Controller
    {
        private readonly IUserService userService;

        protected BaseController()
        {
        }

        protected BaseController(IUserService userService)
            : this()
        {
            this.userService = userService;
        }

        protected IActionResult AuthorizeIfRestaurantCreatorIsCurentUser(int restaurantId)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (this.userService.CheckIfCurrentUserIsNotAuthorByGivenId(restaurantId, currentUserId))
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext?.TraceIdentifier,
                    Message = UnauthorizedMessage,
                    StatusCode = 401,
                });
            }

            return null;
        }
    }
}
