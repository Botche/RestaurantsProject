namespace RestaurantsPlatform.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;

    [Authorize(Roles = AdministratorOrRestaurantOwner)]
    public class ImageRestaurantController : BaseController
    {
        private readonly IRestaurantImageService imageService;
        private readonly IUserService userService;

        public ImageRestaurantController(
            IRestaurantImageService imageService,
            IUserService userService)
        {
            this.imageService = imageService;
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddImageToRestaurant(AddPictureToRestaurantInputModel input)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (this.userService.CheckIfCurrentUserIsAuthorByGivenId(input.Id, currentUserId))
            {
                return this.Unauthorized();
            }

            await this.imageService.AddImageToRestaurantAsync(input.ImageUrl, input.RestaurantName, input.Id);

            return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName });
        }
    }
}
