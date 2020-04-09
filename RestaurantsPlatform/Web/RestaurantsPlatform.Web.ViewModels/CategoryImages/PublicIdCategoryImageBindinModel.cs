namespace RestaurantsPlatform.Web.ViewModels.CategoryImages
{
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class PublicIdCategoryImageBindinModel : IMapFrom<Category>
    {
        public string Name { get; set; }

        public int ImageId { get; set; }
    }
}
