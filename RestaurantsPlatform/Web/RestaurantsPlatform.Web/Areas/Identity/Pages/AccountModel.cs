namespace RestaurantsPlatform.Web.Areas.Identity.Pages
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Users;

    [AllowAnonymous]
    public class AccountModel : PageModel
    {
        private const int FavouritesPerPage = 2;

        private readonly IUserService userService;

        public AccountModel(IUserService userService)
        {
            this.userService = userService;
        }

        public DetailsUserViewModel CurrentUser { get; set; }

        public async Task OnGet(string userName)
        {
            this.CurrentUser = await this.userService.GetUserInfoByUsernameAsync<DetailsUserViewModel>(userName ?? this.User.Identity.Name);

            foreach (var restaurant in this.CurrentUser.FavouriteRestaurants)
            {
                restaurant.IsFavourite = true;
            }
        }
    }
}
