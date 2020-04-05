namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class AuthorIdFromRestaurantBindingModel : IMapFrom<Restaurant>
    {
        public string UserId { get; set; }
    }
}
