namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Category;

    public class UpdateCategoryInputModel
    {
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
    }
}
