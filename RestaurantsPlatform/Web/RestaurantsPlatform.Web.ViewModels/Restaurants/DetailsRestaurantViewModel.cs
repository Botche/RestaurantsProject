namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.RestaurantsImages;

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

        [Required]
        [Url]
        public string ImageUrl { get; set; }

        public string CategoryUrl => this.CategoryName.Replace(' ', '-').ToLower();

        public string RestaurantUrl => this.RestaurantName.Replace(' ', '-').ToLower();

        public IEnumerable<DetailsRestaurantImageViewModel> Images { get; set; }
    }
}
