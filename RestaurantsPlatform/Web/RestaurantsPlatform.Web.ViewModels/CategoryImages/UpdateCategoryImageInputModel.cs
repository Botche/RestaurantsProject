namespace RestaurantsPlatform.Web.ViewModels.CategoryImages
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateCategoryImageInputModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [Url]
        public string ImageUrl { get; set; }
    }
}
