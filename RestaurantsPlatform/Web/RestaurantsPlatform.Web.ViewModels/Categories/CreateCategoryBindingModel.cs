namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Web.Infrastructure;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Category;

    public class CreateCategoryBindingModel
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [DisplayName("Image URL")]
        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
