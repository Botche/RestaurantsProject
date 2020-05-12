namespace RestaurantsPlatform.Web.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Common.StringExtensions;
    using static RestaurantsPlatform.Web.Common.ErrorConstants;
    using static RestaurantsPlatform.Web.Common.NotificationsMessagesContants;

    [Area("Restaurants")]
    public class RestaurantsController : BaseController
    {
        private readonly IRestaurantService restaurantService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        private readonly IFavouriteService favouriteService;

        public RestaurantsController(
            IRestaurantService restaurantService,
            ICategoryService categoryService,
            IUserService userService,
            IConfiguration configuration,
            IFavouriteService favouriteService)
            : base(userService)
        {
            this.restaurantService = restaurantService;
            this.categoryService = categoryService;
            this.userService = userService;
            this.configuration = configuration;
            this.favouriteService = favouriteService;
        }

        public async Task<IActionResult> GetByIdAndName(int id, string name)
        {
            var restaurant = await this.restaurantService.GetByIdAndNameAsync<DetailsRestaurantViewModel>(id, name);

            var result = this.CheckIfValueIsNull(restaurant, PageNotFound, 404);
            if (result != null)
            {
                return result;
            }

            restaurant.IsFavourite =
                this.favouriteService.CheckIfRestaurantIsFavourite(restaurant.Id, this.User.FindFirstValue(ClaimTypes.NameIdentifier));

            this.ViewBag.GoogleMapsApiKey = this.configuration.GetSection("GoogleMaps")["ApiKey"];

            return this.View(restaurant);
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public IActionResult Create()
        {
            var categories = this.categoryService.GetAllCategories<AllCategoriesToCreateRestaurantViewModel>();

            if (!categories.Any())
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext?.TraceIdentifier,
                    Message = NoRegisteredCategories,
                    StatusCode = 404,
                });
            }

            this.ViewBag.Categories = categories;

            return this.View();
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateRestaurantInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.View(input);
            }

            string userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int restaurantId = await this.restaurantService.CreateRestaurantAsync(userId, input.Address, input.CategoryId, input.ContactInfo, input.Description, input.OwnerName, input.RestaurantName, input.WorkingTime);

            var result = this.CheckIfIdIsZero(restaurantId);
            if (result != null)
            {
                return result;
            }

            this.TempData[SuccessNotification] = string.Format(SuccessfullyCreatedRestaurant, input.RestaurantName);
            return this.RedirectToAction("GetByIdAndName", new
            {
                id = restaurantId,
                name = input.RestaurantName.ToSlug(),
            });
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public async Task<IActionResult> Update(int id)
        {
            var restaurant = await this.restaurantService.GetByIdAsync<UpdateRestaurantViewModel>(id);

            var resultForRestaurant = this.CheckIfValueIsNull(restaurant, PageNotFound, 404);
            if (resultForRestaurant != null)
            {
                return resultForRestaurant;
            }

            var result = await this.AuthorizeIfRestaurantCreatorIsCurentUserAsync(id);
            if (result != null)
            {
                return result;
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
                this.TempData[ErrorNotification] = WrontInput;
                return this.View(input);
            }

            var result = await this.AuthorizeIfRestaurantCreatorIsCurentUserAsync(input.Id);
            if (result != null)
            {
                return result;
            }

            int? modelId = await this.restaurantService.UpdateRestaurantAsync(input.Id, input.OwnerName, input.RestaurantName, input.WorkingTime, input.Address, input.ContactInfo, input.Description, input.CategoryId);

            var resultForModel = this.CheckIfValueIsNull(modelId, RestaurantNotFound, 404);
            if (resultForModel != null)
            {
                return resultForModel;
            }

            this.TempData[SuccessNotification] = string.Format(SuccessfullyUpdatedRestaurant, input.RestaurantName);
            return this.RedirectToRoute(
                "restaurant",
                new { id = modelId, name = input.RestaurantName.ToSlug() });
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public async Task<IActionResult> Delete(int id)
        {
            var restaurant = await this.restaurantService.GetByIdAsync<DeleteRestaurantViewModel>(id);

            var resultForRestaurant = this.CheckIfValueIsNull(restaurant, PageNotFound, 404);
            if (resultForRestaurant != null)
            {
                return resultForRestaurant;
            }

            var result = await this.AuthorizeIfRestaurantCreatorIsCurentUserAsync(id);
            if (result != null)
            {
                return result;
            }

            return this.View(restaurant);
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRestaurantInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.View(input);
            }

            var result = await this.AuthorizeIfRestaurantCreatorIsCurentUserAsync(input.Id);
            if (result != null)
            {
                return result;
            }

            var id = await this.restaurantService.DeleteRestaurantByIdAsync(input.Id);

            var resultForRestaurant = this.CheckIfValueIsNull(id, RestaurantNotFound, 404);
            if (resultForRestaurant != null)
            {
                return resultForRestaurant;
            }

            this.TempData[SuccessNotification] = SuccessfullyDeletedRestaurant;
            return this.RedirectToAction("All", "Categories");
        }
    }
}
