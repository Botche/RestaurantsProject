namespace RestaurantsPlatform.Web.Areas.Identity.Pages.Account
{
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using RestaurantsPlatform.Data.Models;

    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender sender;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender)
        {
            this.userManager = userManager;
            this.sender = sender;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email)
        {
            if (email == null)
            {
                return this.RedirectToPage("/Index");
            }

            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with email '{email}'.");
            }

            this.Email = email;

            return this.Page();
        }
    }
}
