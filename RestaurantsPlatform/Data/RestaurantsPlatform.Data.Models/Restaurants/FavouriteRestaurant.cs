namespace RestaurantsPlatform.Data.Models.Restaurants
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Common.Models;

    public class FavouriteRestaurant : BaseModel<string>
    {
        public FavouriteRestaurant()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
