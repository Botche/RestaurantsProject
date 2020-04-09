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

    using static RestaurantsPlatform.Common.GlobalConstants;

    public class RestaurantsController : BaseController
    {
        private readonly IRestaurantService restaurantService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public RestaurantsController(
            IRestaurantService restaurantService,
            ICategoryService categoryService,
            IUserService userService,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            this.restaurantService = restaurantService;
            this.categoryService = categoryService;
            this.userService = userService;
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

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public IActionResult Create()
        {
            var categories = this.categoryService.GetAllCategories<AllCategoriesToCreateRestaurantViewModel>();

            this.ViewBag.Categories = categories;

            return this.View();
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRestaurantInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int restaurantId = await this.restaurantService.CreateRestaurantAsync(userId, input.Address, input.CategoryId, input.ContactInfo, input.Description, input.OwnerName, input.RestaurantName, input.WorkingTime);

            return this.RedirectToAction("GetByIdAndName", new { id = restaurantId, name = input.RestaurantName });
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public IActionResult Update(int id)
        {
            var restaurant = this.restaurantService.GetById<UpdateRestaurantViewModel>(id);

            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (this.userService.CheckIfCurrentUserIsAuthor(restaurant.UserId, currentUserId))
            {
                return this.Unauthorized();
            }

            if (restaurant == null)
            {
                return this.NotFound();
            }

            var categories = this.categoryService.GetAllCategories<AllCategoriesToCreateRestaurantViewModel>();

            this.ViewBag.Categories = categories;

            return this.View(restaurant);
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateRestaurantInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (this.userService.CheckIfCurrentUserIsAuthorByGivenId(input.Id, currentUserId))
            {
                return this.Unauthorized();
            }

            int modelId = await this.restaurantService.UpdateRestaurantAsync(input.Id, input.OwnerName, input.RestaurantName, input.WorkingTime, input.Address, input.ContactInfo, input.Description, input.CategoryId);

            return this.RedirectToRoute(
                "restaurant",
                new { id = modelId, name = input.RestaurantName.ToLower().Replace(' ', '-') });
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public IActionResult Delete(int id)
        {
            var restaurant = this.restaurantService.GetById<DeleteRestaurantViewModel>(id);

            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (this.userService.CheckIfCurrentUserIsAuthor(restaurant.UserId, currentUserId))
            {
                return this.Unauthorized();
            }

            if (restaurant == null)
            {
                return this.NotFound();
            }

            return this.View(restaurant);
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRestaurantInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (this.userService.CheckIfCurrentUserIsAuthorByGivenId(input.Id, currentUserId))
            {
                return this.Unauthorized();
            }

            await this.restaurantService.DeleteRestaurantByIdAsync(input.Id);

            return this.RedirectToAction("All", "Categories");
        }
    }
}
