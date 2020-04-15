namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    public class DetailsCategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [DisplayName("Image Url")]
        public string ImageImageUrl { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public int OldPage { get; set; }

        public string Url => this.Name.Replace(' ', '-').ToLower();

        public IEnumerable<AllRestaurantsViewModel> Restaurants { get; set; }
    }
}
