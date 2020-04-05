namespace RestaurantsPlatform.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService restaurantService;
        private readonly ICategoryService categoryService;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public RestaurantsController(
            IRestaurantService restaurantService,
            ICategoryService categoryService,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            this.restaurantService = restaurantService;
            this.categoryService = categoryService;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public IActionResult GetByIdAndName(int id, string name)
        {
            var restaurant = this.restaurantService.GetByIdAndName<DetailsRestaurantViewModel>(id, name);

            if (restaurant == null)
            {
                return this.NotFound();
            }

            this.ViewBag.GoogleMapsApiKey = this.configuration.GetSection("GoogleMaps")["ApiKey"];

            return this.View(restaurant);
        }

        [Authorize]
        public IActionResult Create()
        {
            var categories = this.categoryService.GetAllCategories<AllCategoriesToCreateRestaurantViewModel>();

            this.ViewBag.Categories = categories;

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRestaurantInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int restaurantId = await this.restaurantService.CreateRestaurant(userId, model.Address, model.CategoryId, model.ContactInfo, model.Description, model.OwnerName, model.RestaurantName, model.WorkingTime);

            return this.RedirectToAction("GetByIdAndName", new { id = restaurantId, name = model.RestaurantName });
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var restaurant = this.restaurantService.GetById<EditRestaurantViewModel>(id);

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (restaurant.UserId != userId)
            {
                return this.Unauthorized();
            }

            if (restaurant == null)
            {
                return this.NotFound();
            }

            return this.View(restaurant);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditRestaurantInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await this.restaurantService.EditRestaurant(userId, model.Id, model.OwnerName, model.RestaurantName, model.WorkingTime, model.Address, model.ContactInfo, model.Description);

            return this.RedirectToRoute(
                "restaurant",
                new { id = model.Id, name = model.RestaurantName.ToLower().Replace(' ', '-') });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var restaurant = this.restaurantService.GetById<EditRestaurantViewModel>(id);

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (restaurant.UserId != userId)
            {
                return this.Unauthorized();
            }

            if (restaurant == null)
            {
                return this.NotFound();
            }

            return this.View(restaurant);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRestaurantInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);


            await this.restaurantService.DeleteRestaurantById(model.Id);

            return this.RedirectToRoute(
                "restaurant",
                new { id = model.Id, name = model.RestaurantName.ToLower().Replace(' ', '-') });
        }
    }
}
