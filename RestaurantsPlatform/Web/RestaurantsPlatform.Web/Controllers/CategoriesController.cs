namespace RestaurantsPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    public class CategoriesController : BaseController
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

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int id = await this.categoryService.CreateCategory(input.Description, input.ImageUrl, input.Name, input.Title);

            return this.RedirectToAction("GetByIdAndName", new { id = id, name = input.Name });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var category = this.categoryService.GetById<UpdateCategoryViewModel>(id);

            return this.View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            int id = await this.categoryService.UpdateCategory(input.Id, input.Description, input.Name, input.Title);

            return this.RedirectToAction("GetByIdAndName", new { id = id, name = input.Name });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var category = this.categoryService.GetById<DeleteCategoryViewModel>(id);

            return this.View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCategoryInputModel input)
        {
            int id = await this.categoryService.DeleteCategory(input.Id);

            return this.RedirectToAction("All");
        }
    }
}
