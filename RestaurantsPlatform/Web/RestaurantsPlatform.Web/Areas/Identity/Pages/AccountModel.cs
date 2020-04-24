namespace RestaurantsPlatform.Web.Areas.Identity.Pages
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Users;

    using static RestaurantsPlatform.Common.GlobalConstants;

    [AllowAnonymous]
    public class AccountModel : PageModel
    {
        private readonly IUserService userService;

        public AccountModel(IUserService userService)
        {
            this.userService = userService;
        }

        public DetailsUserViewModel CurrentUser { get; set; }

        public void OnGet(string userName)
        {
            this.CurrentUser = this.userService.GetUserInfoByUsername<DetailsUserViewModel>(userName ?? this.User.Identity.Name);
        }
    }
}
