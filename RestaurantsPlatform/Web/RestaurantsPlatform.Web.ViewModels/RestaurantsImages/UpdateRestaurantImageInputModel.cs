﻿namespace RestaurantsPlatform.Web.ViewModels.RestaurantsImages
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateRestaurantImageInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string OldImageUrl { get; set; }

        [Required]
        public string RestaurantName { get; set; }
    }
}
