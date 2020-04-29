namespace RestaurantsPlatform.Web.ViewModels.UserImage
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Mapping;

    public class UserImageInputModel : IMapFrom<ApplicationUser>
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
