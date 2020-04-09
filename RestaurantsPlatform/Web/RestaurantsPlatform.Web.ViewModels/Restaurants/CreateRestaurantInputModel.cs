﻿namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using static RestaurantsPlatform.Data.Common.Constants.Models.Restraurant;

    public class CreateRestaurantInputModel : IMapTo<Restaurant>
    {
        [DisplayName("Restaurant name")]
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string RestaurantName { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [DisplayName("Owner name")]
        [MaxLength(OwnerNameMaxLength)]
        public string OwnerName { get; set; }

        [DisplayName("Working time")]
        [RegularExpression(WorkingTimePattern)]
        [Required]
        public string WorkingTime { get; set; }

        [DisplayName("Category")]
        [Required]
        public int CategoryId { get; set; }

        [DisplayName("Contact info")]
        [Required]
        [MaxLength(ContactInfoMaxLength)]
        public string ContactInfo { get; set; }
    }
}
