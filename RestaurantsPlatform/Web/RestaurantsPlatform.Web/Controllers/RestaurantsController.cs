namespace RestaurantsPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService restaurantService;
        private readonly ICategoryService categoryService;

        public RestaurantsController(IRestaurantService restaurantService, ICategoryService categoryService)
        {
            this.restaurantService = restaurantService;
            this.categoryService = categoryService;
        }

        public IActionResult GetById(int id)
        {
            //var restaurant = this.restaurantService.GetById<>();

            //if (restaurant == null)
            //{
            //    return this.NotFound();
            //}

            return this.View();
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
