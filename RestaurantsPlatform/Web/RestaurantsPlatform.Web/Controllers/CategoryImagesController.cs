namespace RestaurantsPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    using static RestaurantsPlatform.Common.GlobalConstants;

    [Authorize(Roles = AdministratorRoleName)]
    public class CategoryImagesController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryImagesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Update(int id, string imageUrl)
        {
            var image = new UpdateCategoryImageViewModel();

            image.CategoryId = id;
            image.ImageUrl = imageUrl;

            return this.View(image);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryImageInputModel input)
        {
            await this.categoryService.UpdateCategoryImageAsync(input.CategoryId, input.ImageUrl);

            return this.RedirectToAction("All", "Categories");
        }
    }
}