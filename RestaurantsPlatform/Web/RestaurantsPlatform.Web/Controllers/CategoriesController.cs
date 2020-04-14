namespace RestaurantsPlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Web.Infrastructure.ErrorConstants;
    using static RestaurantsPlatform.Web.Infrastructure.NotificationsMessagesContants;

    public class CategoriesController : BaseController
    {
        private const int RestaurantsPerPage = 6;
        private const int CategoriesPerPage = 6;

        private readonly ICategoryService categoryService;
        private readonly IRestaurantService restaurantService;

        public CategoriesController(
            ICategoryService categoryService,
            IRestaurantService restaurantService,
            IUserService userService)
            : base(userService)
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
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = PageNotFound,
                    StatusCode = 404,
                });
            }

            category.Restaurants = this.restaurantService.GetRestaurantsByCategoryId<AllRestaurantsViewModel>(category.Id, RestaurantsPerPage, (page - 1) * RestaurantsPerPage);

            var count = this.restaurantService.GetCountByCategoryId(category.Id);
            category.PagesCount = (int)Math.Ceiling((double)count / RestaurantsPerPage);
            if (category.PagesCount == 0)
            {
                category.PagesCount = 1;
            }

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

            return this.RedirectToRoute("category", new { id, name = input.Name.ToLower().Replace(' ', '-') });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Update(int id)
        {
            var category = this.categoryService.GetById<UpdateCategoryViewModel>(id);

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

            return this.RedirectToRoute("category", new { id, name = input.Name.ToLower().Replace(' ', '-') });
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Delete(int id)
        {
            var category = this.categoryService.GetById<DeleteCategoryViewModel>(id);

            return this.View(category);
        }

        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCategoryInputModel input)
        {
            await this.categoryService.DeleteCategoryAsync(input.Id);

            this.TempData[SuccessNotification] = SuccessfullyDeletedCategory;

            return this.RedirectToAction("All");
        }
    }
}
