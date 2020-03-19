namespace RestaurantsPlatform.Data.Models.Restaurants
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Common.Models;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Image;

    public class Image : BaseDeletableModel<string>
    {
        [Required]
        [MaxLength(AlternativeTextMaxLength)]
        public string AlternativeText { get; set; }

        [Required]
        public string Source { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public string RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}
