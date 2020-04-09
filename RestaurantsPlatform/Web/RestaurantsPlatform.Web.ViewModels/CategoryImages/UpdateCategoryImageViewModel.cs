namespace RestaurantsPlatform.Web.ViewModels.CategoryImages
{
    using System.ComponentModel;

    public class UpdateCategoryImageViewModel
    {
        public int CategoryId { get; set; }

        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }
    }
}
