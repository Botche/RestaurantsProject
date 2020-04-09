namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.ComponentModel;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class DeleteCategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [DisplayName("Image URL")]
        public string ImageImageUrl { get; set; }

        public string Url => this.Name.Replace(' ', '-').ToLower();
    }
}
