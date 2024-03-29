﻿namespace RestaurantsPlatform.Data.Models.Restaurants
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Common.Models;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Category;

    public class Category : DeletableEntity<int>
    {
        public Category()
        {
            this.Restaurants = new HashSet<Restaurant>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public int ImageId { get; set; }

        public virtual CategoryImage Image { get; set; }

        public virtual IEnumerable<Restaurant> Restaurants { get; set; }
    }
}
