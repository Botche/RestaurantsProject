namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using System.Collections.Generic;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.Images;

    public class AllRestaurantsViewModel : IMapFrom<Restaurant>
    {
        public int Id { get; set; }

        public string RestaurantName { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string WorkingTime { get; set; }

        public virtual IEnumerable<DetailsRestaurantImageViewModel> Images { get; set; }

        public string ParsedUrlName => this.RestaurantName.Replace(' ', '-').ToLower();
    }
}
