namespace RestaurantsPlatform.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.Users;

    public class DashboardController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdministrationService administrationService;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            IAdministrationService administrationService)
        {
            this.userManager = userManager;
            this.administrationService = administrationService;
        }

        public IActionResult Index()
        {
            return this.RedirectToActionPermanent("Users");
        }

        public IActionResult Users()
        {
            var users = this.userManager.Users.To<UserInformationViewModel>()
                .OrderBy(user => user.Email)
                .ToList();

            return this.View(users);
        }

        public async Task<IActionResult> Promote(UserPromoteDemoteInputModel input)
        {
            await this.administrationService.PromoteAsync(input.Id, input.RoleName);

            return this.RedirectToAction("Users");
        }

        public async Task<IActionResult> Demote(UserPromoteDemoteInputModel input)
        {
            await this.administrationService.DemoteAsync(input.Id, input.RoleName);

            return this.RedirectToAction("Users");
        }

        public IActionResult Ban(string id)
        {
            this.administrationService.BanAsync(id);

            return this.RedirectToAction("Users");
        }
    }
}
