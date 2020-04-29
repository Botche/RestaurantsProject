namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteRestaurantInputModel
    {
        [Required]
        public int Id { get; set; }
    }
}
