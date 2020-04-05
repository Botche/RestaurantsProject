namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class DetailsRestaurantViewModel : IMapFrom<Restaurant>
    {
        public int Id { get; set; }

        public string RestaurantName { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        public string OwnerName { get; set; }

        public string WorkingTime { get; set; }

        public string ContactInfo { get; set; }

        public string UserId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
