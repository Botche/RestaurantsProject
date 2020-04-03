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

    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult All(int? count = null)
        {
            IEnumerable<AllCategoriesViewModel> categories = this.categoryService.GetAllCategories<AllCategoriesViewModel>(count);

            return this.View(categories);
        }

        public IActionResult ByName(int id)
        {
            var category = this.categoryService.GetById<DetailsCategoryViewModel>(id);

            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }
    }
}