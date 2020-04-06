namespace RestaurantsPlatform.Web.ViewModels.Images
{
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class CategoryImageBindingModel : IMapTo<CategoryImage>
    {
        public string ImageUrl { get; set; }

        public string PublicId { get; set; }
    }
}
