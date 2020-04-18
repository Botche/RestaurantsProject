namespace RestaurantsPlatform.Web.Areas.Restaurants.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Favourites;

    [ApiController]
    [Route("api/[controller]")]
    public class FavouriteRestaurantsController : ControllerBase
    {
        private readonly IFavouriteService favouriteService;
        private readonly UserManager<ApplicationUser> userManager;

        public FavouriteRestaurantsController(
            IFavouriteService favouriteService,
            UserManager<ApplicationUser> userManager)
        {
            this.favouriteService = favouriteService;
            this.userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Post(FavouriteRestaurantsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.favouriteService.FavouriteAsync(input.RestaurantId, userId, input.IsFavourite);

            return this.Ok();
        }
    }
}
