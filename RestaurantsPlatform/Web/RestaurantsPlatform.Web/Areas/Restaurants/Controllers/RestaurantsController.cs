namespace RestaurantsPlatform.Web.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Web.Infrastructure.ErrorConstants;
    using static RestaurantsPlatform.Web.Infrastructure.NotificationsMessagesContants;

    [Area("Restaurants")]
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
            : base(userService)
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
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = PageNotFound,
                    StatusCode = 404,
                });
            }

            this.ViewBag.GoogleMapsApiKey = this.configuration.GetSection("GoogleMaps")["ApiKey"];

            return this.View(restaurant);
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public IActionResult Create()
        {
            var categories = this.categoryService.GetAllCategories<AllCategoriesToCreateRestaurantViewModel>();

            if (categories == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
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

            if (restaurantId == 0)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                });
            }

            this.TempData[SuccessNotification] = string.Format(SuccessfullyCreatedRestaurant, input.RestaurantName);
            return this.RedirectToAction("GetByIdAndName", new
            {
                id = restaurantId,
                name = input.RestaurantName.ToLower().Replace(' ', '-'),
            });
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public IActionResult Update(int id)
        {
            var restaurant = this.restaurantService.GetById<UpdateRestaurantViewModel>(id);

            if (restaurant == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = PageNotFound,
                    StatusCode = 404,
                });
            }

            var result = this.AuthorizeIfRestaurantCreatorIsCurentUser(id);
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

            var result = this.AuthorizeIfRestaurantCreatorIsCurentUser(input.Id);
            if (result != null)
            {
                return result;
            }

            int? modelId = await this.restaurantService.UpdateRestaurantAsync(input.Id, input.OwnerName, input.RestaurantName, input.WorkingTime, input.Address, input.ContactInfo, input.Description, input.CategoryId);

            if (modelId == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = RestaurantNotFound,
                    StatusCode = 404,
                });
            }

            this.TempData[SuccessNotification] = string.Format(SuccessfullyUpdatedRestaurant, input.RestaurantName);
            return this.RedirectToRoute(
                "restaurant",
                new { id = modelId, name = input.RestaurantName.ToLower().Replace(' ', '-') });
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        public IActionResult Delete(int id)
        {
            var restaurant = this.restaurantService.GetById<DeleteRestaurantViewModel>(id);

            if (restaurant == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = PageNotFound,
                    StatusCode = 404,
                });
            }

            var result = this.AuthorizeIfRestaurantCreatorIsCurentUser(id);
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

            var result = this.AuthorizeIfRestaurantCreatorIsCurentUser(input.Id);
            if (result != null)
            {
                return result;
            }

            var id = await this.restaurantService.DeleteRestaurantByIdAsync(input.Id);

            if (id == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = RestaurantNotFound,
                    StatusCode = 404,
                });
            }

            this.TempData[SuccessNotification] = SuccessfullyDeletedRestaurant;
            return this.RedirectToAction("All", "Categories");
        }
    }
}
