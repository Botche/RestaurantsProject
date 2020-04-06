namespace RestaurantsPlatform.Data.Models.Restaurants
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Common.Models;

    public class CategoryImage : IDeletableEntity<int>
    {
        [Required]
        [Url]
        public string ImageUrl { get; set; }

        [Required]
        public string PublicId { get; set; }
    }
}
