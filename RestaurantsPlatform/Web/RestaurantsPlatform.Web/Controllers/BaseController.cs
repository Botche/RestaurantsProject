namespace RestaurantsPlatform.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels;

    using static RestaurantsPlatform.Web.Common.ErrorConstants;

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

        protected async Task<IActionResult> AuthorizeIfRestaurantCreatorIsCurentUserAsync(int restaurantId)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await this.userService.CheckIfCurrentUserIsNotAuthorByGivenIdAsync(restaurantId, currentUserId))
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

        protected int CheckIfGivenPageIsBelowOne(int page)
        {
            if (page < 1)
            {
                page = 1;
            }

            return page;
        }

        protected IActionResult CheckIfValueIsNull(object obj, string message, int statusCode)
        {
            if (obj == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext?.TraceIdentifier,
                    Message = message,
                    StatusCode = statusCode,
                });
            }

            return null;
        }

        protected IActionResult CheckIfIdIsZero(int? id)
        {
            if (id == 0)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                });
            }

            return null;
        }
    }
}
