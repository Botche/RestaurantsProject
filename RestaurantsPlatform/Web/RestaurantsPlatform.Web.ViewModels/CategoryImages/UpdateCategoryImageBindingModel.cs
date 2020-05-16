namespace RestaurantsPlatform.Web.ViewModels.CategoryImages
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Web.Infrastructure;

    public class UpdateCategoryImageBindingModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
