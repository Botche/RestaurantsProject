namespace RestaurantsPlatform.Web.Areas.Restaurants.Controllers
{
    using System.Security.Claims;
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

        public FavouriteRestaurantsController(IFavouriteService favouriteService)
        {
            this.favouriteService = favouriteService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(FavouriteRestaurantsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.favouriteService.FavouriteAsync(input.RestaurantId, userId, input.IsFavourite);

            return this.Ok();
        }
    }
}
