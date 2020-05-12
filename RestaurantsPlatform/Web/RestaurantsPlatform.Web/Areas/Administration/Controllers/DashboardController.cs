namespace RestaurantsPlatform.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Messaging;
    using RestaurantsPlatform.Web.ViewModels.Users;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Web.Common.EmailSentInformationConstants;

    [Area("Administration")]
    public class DashboardController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAdministrationService administrationService;
        private readonly IUserService userService;
        private readonly IEmailSender emailSender;

        public DashboardController(
            UserManager<ApplicationUser> userManager,
            IAdministrationService administrationService,
            IUserService userService,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.administrationService = administrationService;
            this.userService = userService;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.RedirectToActionPermanent("Users");
        }

        public IActionResult Users()
        {
            var users = this.userService.GetAllUsersWithDeleted<UserInformationViewModel>()
                .OrderBy(user => user.Email);

            return this.View(users);
        }

        public async Task<IActionResult> Promote(UserPromoteDemoteInputModel input)
        {
            await this.administrationService.PromoteAsync(input.Id, input.RoleName);
            var user = await this.userManager.FindByIdAsync(input.Id);

            await this.emailSender.SendEmailAsync(
                OwnersEmail,
                OwnersName,
                user.Email,
                PromotedEmailSubject,
                PromotedEmailHtmlContent);

            return this.RedirectToRoute("areaRoute", new { area = "Administration", controller = "Dashboard", action = "Users" });
        }

        public async Task<IActionResult> Demote(UserPromoteDemoteInputModel input)
        {
            await this.administrationService.DemoteAsync(input.Id, input.RoleName);
            var user = await this.userManager.FindByIdAsync(input.Id);

            await this.emailSender.SendEmailAsync(
                OwnersEmail,
                OwnersName,
                user.Email,
                DemotedEmailSubject,
                DemotedEmailHtmlContent);

            return this.RedirectToRoute("areaRoute", new { area = "Administration", controller = "Dashboard", action = "Users" });
        }

        public async Task<IActionResult> Ban(string id)
        {
            await this.administrationService.BanAsync(id);

            var user = await this.userManager.FindByIdAsync(id);

            await this.emailSender.SendEmailAsync(
                OwnersEmail,
                OwnersName,
                user.Email,
                BannedEmailSubject,
                BannedEmailHtmlContent);

            return this.RedirectToRoute("areaRoute", new { area = "Administration", controller = "Dashboard", action = "Users" });
        }

        public async Task<IActionResult> UnBan(string id)
        {
            await this.administrationService.UnBanAsync(id);

            var user = await this.userManager.FindByIdAsync(id);

            await this.emailSender.SendEmailAsync(
                OwnersEmail,
                OwnersName,
                user.Email,
                UnBannedEmailSubject,
                UnBannedEmailHtmlContent);

            return this.RedirectToRoute("areaRoute", new { area = "Administration", controller = "Dashboard", action = "Users" });
        }
    }
}
