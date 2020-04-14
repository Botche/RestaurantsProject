namespace RestaurantsPlatform.Data.Models
{
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public ApplicationRole Role { get; set; }

        public ApplicationUser User { get; set; }
    }
}
