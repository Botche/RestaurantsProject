﻿namespace RestaurantsPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    [Authorize]
    public class CategoriesController : Controller
    {
        private const int RestaurantsPerPage = 6;

        private readonly ICategoryService categoryService;
        private readonly IRestaurantService restaurantService;

        public CategoriesController(
            ICategoryService categoryService,
            IRestaurantService restaurantService)
        {
            this.categoryService = categoryService;
            this.restaurantService = restaurantService;
        }

        public IActionResult All(int? count = null)
        {
            IEnumerable<AllCategoriesViewModel> categories = this.categoryService.GetAllCategories<AllCategoriesViewModel>(count);

            return this.View(categories);
        }

        public IActionResult GetByIdAndName(int id, string name, int page = 1)
        {
            var category = this.categoryService.GetByIdAndName<DetailsCategoryViewModel>(id, name);

            if (category == null)
            {
                return this.NotFound();
            }

            category.Restaurants = this.restaurantService.GetByCategoryId<AllRestaurantsViewModel>(category.Id, RestaurantsPerPage, (page - 1) * RestaurantsPerPage);

            var count = this.restaurantService.GetCountByCategoryId(category.Id);
            category.PagesCount = (int)Math.Ceiling((double)count / RestaurantsPerPage);
            if (category.PagesCount == 0)
            {
                category.PagesCount = 1;
            }

            return this.View(category);
        }
    }
}