namespace RestaurantsPlatform.Web.Areas.Identity.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;
    using RestaurantsPlatform.Web.ViewModels.UserImage;

    [Area("Identity")]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> DeleteImage(string userName)
        {
            if (userName != this.User.Identity.Name)
            {
                return this.Unauthorized();
            }

            var imageUrl = await this.userService.DeleteProfilePictureByUsernameAsync(userName);

            if (imageUrl == null)
            {
                return this.BadRequest();
            }

            return this.Ok(imageUrl);
        }

        public IActionResult UpdateImage(string userName)
        {
            if (userName != this.User.Identity.Name)
            {
                return this.Unauthorized();
            }

            var imageUrl = this.userService.GetUserImage(userName);

            if (imageUrl == null)
            {
                return this.BadRequest();
            }

            return this.View(imageUrl);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateImage(UserImageInputModel input)
        {
            if (input.Username != this.User.Identity.Name)
            {
                return this.Unauthorized();
            }

            var id = await this.userService.UpdateProfilePictureByAsync(input.Username, input.ImageUrl);

            if (id == null)
            {
                return this.BadRequest();
            }

            return this.Redirect("/Identity/Account");
        }
    }
}
