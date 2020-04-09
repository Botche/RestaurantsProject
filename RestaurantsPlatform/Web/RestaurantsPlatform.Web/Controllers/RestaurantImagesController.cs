namespace RestaurantsPlatform.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;
    using RestaurantsPlatform.Web.ViewModels.RestaurantsImages;
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

        public IActionResult Update(int id, string imageUrl)
        {
            var restaurant = this.restaurantService.GetById<UpdateRestaurantImageViewModel>(id);
            restaurant.ImageUrl = imageUrl;

            return this.View(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRestaurantImageInputModel input)
        {
            await this.imageService
                .UpdateRestaurantImageAsync(input.Id, input.RestaurantName, input.ImageUrl, input.OldImageUrl);

            return this.RedirectToAction("Gallery", new { id = input.Id });
        }

        public async Task<IActionResult> Delete(int id, string imageUrl)
        {
            await this.restaurantService.DeleteImageByRestaurantIdAsync(id, imageUrl);

            return this.RedirectToAction("Gallery", new { id });
        }
    }
}
