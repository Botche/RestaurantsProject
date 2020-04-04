namespace RestaurantsPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService restaurantService;
        private readonly ICategoryService categoryService;
        private readonly IConfiguration configuration;

        public RestaurantsController(IRestaurantService restaurantService, ICategoryService categoryService, IConfiguration configuration)
        {
            this.restaurantService = restaurantService;
            this.categoryService = categoryService;
            this.configuration = configuration;
        }

        public IActionResult GetById(int id)
        {
            var restaurant = this.restaurantService.GetById<DetailsRestaurantViewModel>(id);

            if (restaurant == null)
            {
                return this.NotFound();
            }

            this.ViewBag.MapBoxApiKey = this.configuration.GetSection("MapBox")["ApiKey"];

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
            await this.restaurantService.CreateRestaurant(model.Address, model.CategoryId, model.ContactInfo, model.Description, model.OwnerName, model.RestaurantName, model.WorkingTime);

            return this.RedirectToAction("GetById", model.CategoryId);
        }
    }
}
