namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Category;

    public class UpdateCategoryViewModel : IMapFrom<Category>
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

        public string Url => this.Name.Replace(' ', '-').ToLower();
    }
}
