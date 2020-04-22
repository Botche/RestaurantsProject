namespace RestaurantsPlatform.Web.Areas.Categories.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Web.Infrastructure.NotificationsMessagesContants;

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
            var image = new UpdateCategoryImageViewModel
            {
                CategoryId = id,
                ImageUrl = imageUrl,
            };

            return this.View(image);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryImageInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.View(input);
            }

            await this.categoryService.UpdateCategoryImageAsync(input.CategoryId, input.ImageUrl);

            this.TempData[SuccessNotification] = SuccessfullyUpdatedImage;
            return this.RedirectToAction("All", "Categories");
        }
    }
}
