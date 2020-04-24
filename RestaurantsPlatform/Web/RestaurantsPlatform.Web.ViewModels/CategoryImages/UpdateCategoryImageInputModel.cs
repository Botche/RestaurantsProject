namespace RestaurantsPlatform.Web.ViewModels.CategoryImages
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Web.Infrastructure;

    public class UpdateCategoryImageInputModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
