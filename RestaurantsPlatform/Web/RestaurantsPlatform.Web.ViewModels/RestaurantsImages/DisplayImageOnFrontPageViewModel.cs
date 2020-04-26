namespace RestaurantsPlatform.Web.ViewModels.RestaurantsImages
{
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class DisplayImageOnFrontPageViewModel : IMapFrom<RestaurantImage>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public int RestaurantId { get; set; }

        public string RestaurantRestaurantName { get; set; }
    }
}
