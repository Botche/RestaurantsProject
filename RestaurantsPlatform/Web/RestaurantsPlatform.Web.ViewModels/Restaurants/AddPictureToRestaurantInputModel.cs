using System.ComponentModel.DataAnnotations;
using RestaurantsPlatform.Web.Infrastructure;

namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    public class AddPictureToRestaurantInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string RestaurantName { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
