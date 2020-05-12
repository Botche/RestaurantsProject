namespace RestaurantsPlatform.Web.Areas.Categories.Controllers
{
    using System;
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Common.StringExtensions;
    using static RestaurantsPlatform.Web.Common.ErrorConstants;
    using static RestaurantsPlatform.Web.Common.NotificationsMessagesContants;

    [Area("Categories")]
    public class CategoriesController : BaseController
    {
        private const int RestaurantsPerPage = 4;
        private const int CategoriesPerPage = 6;

        private readonly ICategoryService categoryService;
        private readonly IRestaurantService restaurantService;
        private readonly IFavouriteService favouriteService;

        public CategoriesController(
            ICategoryService categoryService,
            IRestaurantService restaurantService,
            IFavouriteService favouriteService)
        {
            this.categoryService = categoryService;
            this.restaurantService = restaurantService;
            this.favouriteService = favouriteService;
        }

        public IActionResult All(int page = 1)
        {
            page = this.CheckIfGivenPageIsBelowOne(page);

            var categories = this.categoryService.GetAllCategoriesWithPage<DetailsAllCategoriesViewModel>(
                    CategoriesPerPage,
                    (page - 1) * CategoriesPerPage);

            var count = this.categoryService.GetCountOfAllCategories();
            var allCategoriesViewModel = new AllCategoriesViewModel
            {
                Categories = categories,
                CurrentPage = page,
                PagesCount = (int)Math.Ceiling((double)count / CategoriesPerPage),
            };

            if (allCategoriesViewModel.PagesCount == 0)
            {
                allCategoriesViewModel.PagesCount = 1;
            }

            return this.View(allCategoriesViewModel);
        }

        public async Task<IActionResult> GetByIdAndName(int id, string name, int page = 1)
        {
            page = this.CheckIfGivenPageIsBelowOne(page);

            var category = await this.categoryService.GetByIdAndNameAsync<DetailsCategoryViewModel>(id, name);

            var result = this.CheckIfValueIsNull(category, PageNotFound, 404);
            if (result != null)
            {
                return result;
            }

            category.Restaurants = this.restaurantService.GetRestaurantsByCategoryId<AllRestaurantsViewModel>(category.Id, RestaurantsPerPage, (page - 1) * RestaurantsPerPage);

            foreach (var restaurant in category.Restaurants)
            {
                restaurant.IsFavourite = this.favouriteService
                    .CheckIfRestaurantIsFavourite(restaurant.Id, this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }

            category.CurrentPage = page;

            var count = this.restaurantService.GetCountByCategoryId(category.Id);
            this.CalculatePagesCount(category, count);

            return this.View(category);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.View(input);
            }

            int id = await this.categoryService.CreateCategoryAsync(input.Description, input.ImageUrl, input.Name, input.Title);

            this.TempData[SuccessNotification] = string.Format(SuccessfullyCreatedCategory, input.Name);

            return this.RedirectToRoute("category", new { id, name = input.Name.ToSlug() });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Update(int id)
        {
            var category = await this.categoryService.GetByIdAsync<UpdateCategoryViewModel>(id);

            return this.View(category);
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.View(input);
            }

            int id = await this.categoryService.UpdateCategoryAsync(input.Id, input.Description, input.Name, input.Title);

            this.TempData[SuccessNotification] = string.Format(SuccessfullyUpdatedCategory, input.Name);

            return this.RedirectToRoute("category", new { id, name = input.Name.ToSlug() });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await this.categoryService.GetByIdAsync<DeleteCategoryViewModel>(id);

            return this.View(category);
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCategoryInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoryService.DeleteCategoryAsync(input.Id);

            this.TempData[SuccessNotification] = SuccessfullyDeletedCategory;

            return this.RedirectToAction("All");
        }

        private void CalculatePagesCount(DetailsCategoryViewModel category, int count)
        {
            category.PagesCount = (int)Math.Ceiling((double)count / RestaurantsPerPage);
            if (category.PagesCount == 0)
            {
                category.PagesCount = 1;
            }
        }
    }
}
