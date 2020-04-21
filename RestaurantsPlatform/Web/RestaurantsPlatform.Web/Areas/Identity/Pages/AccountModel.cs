namespace RestaurantsPlatform.Web.Areas.Identity.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.Users;

    using static RestaurantsPlatform.Common.GlobalConstants;

    [Authorize]
    public class AccountModel : PageModel
    {
        private readonly IUserService userService;

        public AccountModel(IUserService userService)
        {
            this.userService = userService;
        }

        public string ImageUrl => DefaultProfilePicture;

        public DetailsUserViewModel CurrentUser { get; set; }

        public void OnGet()
        {
            this.CurrentUser = this.userService.GetCurrentUserInfo<DetailsUserViewModel>(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
