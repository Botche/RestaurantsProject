namespace RestaurantsPlatform.Web.ViewModels.UserImage
{
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Mapping;

    public class UserImageInputModel : IMapFrom<ApplicationUser>
    {
        public string Username { get; set; }

        public string ImageUrl { get; set; }
    }
}
