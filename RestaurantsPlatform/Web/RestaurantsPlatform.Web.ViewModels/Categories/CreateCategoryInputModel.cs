namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Category;

    public class CreateCategoryInputModel
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
    }
}
