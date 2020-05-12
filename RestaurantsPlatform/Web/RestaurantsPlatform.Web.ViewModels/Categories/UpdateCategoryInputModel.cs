namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Web.Infrastructure;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Category;

    public class UpdateCategoryInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
