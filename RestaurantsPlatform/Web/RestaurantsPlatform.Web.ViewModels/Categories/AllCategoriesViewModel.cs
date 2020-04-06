namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class AllCategoriesViewModel : IMapFrom<Category>
    {
        private const int ShortDescriptionLength = 125;

        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription =>
             this.Description?.Length > ShortDescriptionLength
                 ? this.Description?.Substring(0, ShortDescriptionLength) + "..."
                 : this.Description;

        public string ImageImageUrl { get; set; }

        public int RestaurantsCount { get; set; }

        public string UrlName => this.Name.Replace(' ', '-').ToLower();
    }
}
