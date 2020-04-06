namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using System.ComponentModel;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class DeleteRestaurantViewModel : IMapFrom<Restaurant>
    {
        public int Id { get; set; }

        [DisplayName("Restaurant Name")]
        public string RestaurantName { get; set; }

        public string Description { get; set; }

        public string Address { get; set; }

        [DisplayName("Owner Name")]
        public string OwnerName { get; set; }

        [DisplayName("Working Time")]
        public string WorkingTime { get; set; }

        [DisplayName("Contact Info")]
        public string ContactInfo { get; set; }

        public string UserId { get; set; }

        public string Url => this.RestaurantName.Replace(' ', '-').ToLower();
    }
}
