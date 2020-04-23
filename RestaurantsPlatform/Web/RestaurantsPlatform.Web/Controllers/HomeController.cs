namespace RestaurantsPlatform.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Web;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Messaging;
    using RestaurantsPlatform.Web.ViewModels;

    using static RestaurantsPlatform.Common.GlobalConstants;
    using static RestaurantsPlatform.Web.Infrastructure.NotificationsMessagesContants;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;

        public HomeController(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        public IActionResult Contact()
        {
            var model = new ContactInputModel()
            {
                EmailTo = OwnersEmail,
                Name = this.User.Identity.Name ?? "Unknown",
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.emailSender.SendEmailAsync(
                RealUserEmailOne,
                input.Name,
                OwnersEmail,
                input.Subject,
                HttpUtility.HtmlEncode(input.Content));

            this.TempData[SuccessNotification] = ContactResponseMessage;
            return this.RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
