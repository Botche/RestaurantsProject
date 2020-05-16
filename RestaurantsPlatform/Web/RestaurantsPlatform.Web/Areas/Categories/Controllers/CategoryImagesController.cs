namespace RestaurantsPlatform.Web.Areas.Categories.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Web.Common.NotificationsMessagesContants;

    [Area("Categories")]
    [Authorize(Roles = AdministratorRoleName)]
    public class CategoryImagesController : BaseController
    {
        private readonly ICategoryService categoryService;

        public CategoryImagesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public IActionResult Update(int id, string imageUrl)
        {
            var image = new UpdateCategoryImageBindingModel
            {
                CategoryId = id,
                ImageUrl = imageUrl,
            };

            return this.View(image);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryImageBindingModel input)
        {
            if (!this.ModelState.IsValid)
            {
                foreach (var modelState in this.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        this.TempData[ErrorNotification] = error.ErrorMessage;
                    }
                }

                return this.View(input);
            }

            await this.categoryService.UpdateCategoryImageAsync(input.CategoryId, input.ImageUrl);

            this.TempData[SuccessNotification] = SuccessfullyUpdatedImage;
            return this.RedirectToAction("All", "Categories");
        }
    }
}
