namespace RestaurantsPlatform.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Images;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;

    public class RestaurantImagesController : BaseController
    {
        private readonly IRestaurantImageService imageService;
        private readonly IUserService userService;
        private readonly IRestaurantService restaurantService;

        public RestaurantImagesController(
            IRestaurantImageService imageService,
            IUserService userService,
            IRestaurantService restaurantService)
        {
            this.imageService = imageService;
            this.userService = userService;
            this.restaurantService = restaurantService;
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
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

        public IActionResult Gallery(int id)
        {
            var restaurantImages = this.restaurantService.GetById<AllImagesFromRestaurantViewModel>(id);

            if (restaurantImages == null)
            {
                return this.NotFound();
            }

            return this.View(restaurantImages);
        }
    }
}
