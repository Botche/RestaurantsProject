namespace RestaurantsPlatform.Web.ViewModels.RestaurantsImages
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.Infrastructure;

    public class UpdateRestaurantImageViewModel : IMapFrom<Restaurant>
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string RestaurantName { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string Url => this.RestaurantName.Replace(' ', '-').ToLower();

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
