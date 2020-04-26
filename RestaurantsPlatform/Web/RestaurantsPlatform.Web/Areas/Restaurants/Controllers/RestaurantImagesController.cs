namespace RestaurantsPlatform.Web.Areas.Restaurants.Controllers
{
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;
    using RestaurantsPlatform.Web.ViewModels.RestaurantsImages;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Common.StringExtensions;
    using static RestaurantsPlatform.Web.Infrastructure.ErrorConstants;
    using static RestaurantsPlatform.Web.Infrastructure.NotificationsMessagesContants;

    [Area("Restaurants")]
    public class RestaurantImagesController : BaseController
    {
        private readonly IRestaurantImageService imageService;
        private readonly IUserService userService;
        private readonly IRestaurantService restaurantService;

        public RestaurantImagesController(
            IRestaurantImageService imageService,
            IUserService userService,
            IRestaurantService restaurantService)
            : base(userService)
        {
            this.imageService = imageService;
            this.userService = userService;
            this.restaurantService = restaurantService;
        }

        [Authorize(Roles = AdministratorOrRestaurantOwner)]
        [HttpPost]
        public async Task<IActionResult> AddImageToRestaurant(AddPictureToRestaurantInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName.ToSlug() });
            }

            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await this.userService.CheckIfCurrentUserIsNotAuthorByGivenIdAsync(input.Id, currentUserId))
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = UnauthorizedMessage,
                    StatusCode = 401,
                });
            }

            var id = await this.imageService.AddImageToRestaurantAsync(input.ImageUrl, input.RestaurantName, input.Id);

            if (id == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = ImageNotFound,
                    StatusCode = 404,
                });
            }

            this.TempData[SuccessNotification] = string.Format(SuccessfullyAddedImageToRestaurant, input.RestaurantName);
            return this.RedirectToRoute("restaurant", new { id = input.Id, name = input.RestaurantName.ToSlug() });
        }

        public async Task<IActionResult> Gallery(int id)
        {
            var restaurantImages = await this.restaurantService.GetByIdAsync<AllImagesFromRestaurantViewModel>(id);

            if (restaurantImages == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext?.TraceIdentifier,
                    Message = PageNotFound,
                    StatusCode = 404,
                });
            }

            return this.View(restaurantImages);
        }

        public async Task<IActionResult> UpdateAsync(int id, string imageUrl)
        {
            var result = await this.AuthorizeIfRestaurantCreatorIsCurentUserAsync(id);
            if (result != null)
            {
                return result;
            }

            var restaurant = await this.restaurantService.GetRestaurantByIdWithImageAsync<UpdateRestaurantImageViewModel>(id, imageUrl);

            if (restaurant == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext?.TraceIdentifier,
                    Message = PageNotFound,
                    StatusCode = 404,
                });
            }

            restaurant.ImageUrl = imageUrl;

            return this.View(restaurant);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRestaurantImageInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData[ErrorNotification] = WrontInput;
                return this.View(input);
            }

            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await this.userService.CheckIfCurrentUserIsNotAuthorByGivenIdAsync(input.Id, currentUserId))
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = UnauthorizedMessage,
                    StatusCode = 401,
                });
            }

            var updatedId = await this.imageService
                .UpdateRestaurantImageAsync(input.Id, input.RestaurantName, input.ImageUrl, input.OldImageUrl);

            if (updatedId == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = PageNotFound,
                    StatusCode = 404,
                });
            }

            this.TempData[SuccessNotification] = SuccessfullyUpdatedImage;
            return this.RedirectToAction("Gallery", new { id = input.Id });
        }

        public async Task<IActionResult> Delete(int id, string imageUrl)
        {
            string currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (await this.userService.CheckIfCurrentUserIsNotAuthorByGivenIdAsync(id, currentUserId))
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = UnauthorizedMessage,
                    StatusCode = 401,
                });
            }

            var deletedImageCount = await this.restaurantService.DeleteImageByRestaurantIdAsync(id, imageUrl);

            if (deletedImageCount == null)
            {
                return this.View(ErrorViewName, new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier,
                    Message = PageNotFound,
                    StatusCode = 404,
                });
            }

            this.TempData[SuccessNotification] = SuccessfullyDeletedImage;
            return this.RedirectToAction("Gallery", new { id });
        }
    }
}
