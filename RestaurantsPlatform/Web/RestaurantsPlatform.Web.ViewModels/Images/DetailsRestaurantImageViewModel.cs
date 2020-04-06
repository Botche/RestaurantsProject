﻿namespace RestaurantsPlatform.Web.ViewModels.Images
{
    using System.ComponentModel;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class DetailsRestaurantImageViewModel : IMapFrom<RestaurantImage>, IMapTo<RestaurantImage>
    {
        [DisplayName("Image Url")]
        public string ImageUrl { get; set; }
    }
}
