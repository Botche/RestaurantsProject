namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteCategoryInputModel
    {
        [Required]
        public int Id { get; set; }
    }
}
