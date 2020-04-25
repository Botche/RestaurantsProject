namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Web.Infrastructure;

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
