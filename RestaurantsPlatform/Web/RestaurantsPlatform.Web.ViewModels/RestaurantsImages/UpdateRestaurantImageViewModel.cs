namespace RestaurantsPlatform.Web.ViewModels.RestaurantsImages
{
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class UpdateRestaurantImageViewModel : IMapFrom<Restaurant>
    {
        public int Id { get; set; }

        public string RestaurantName { get; set; }

        public string ImageUrl { get; set; }

        public string Url => this.RestaurantName.Replace(' ', '-').ToLower();
    }
}
