namespace RestaurantsPlatform.Web.ViewModels.Favourites
{
    using System.ComponentModel.DataAnnotations;

    public class FavouriteRestaurantsInputModel
    {
        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public bool IsFavourite { get; set; }
    }
}
