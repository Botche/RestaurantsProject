namespace RestaurantsPlatform.Web.ViewModels.Images
{
    using System.Collections.Generic;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class AllImagesFromRestaurantViewModel : IMapFrom<Restaurant>, IMapFrom<Category>
    {
        public IEnumerable<DetailsRestaurantImageViewModel> Images { get; set; }

        public int Id { get; set; }

        public string RestaurantName { get; set; }
    }
}
