namespace RestaurantsPlatform.Data.Models.Restaurants
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using RestaurantsPlatform.Data.Common.Models;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Restraurant;

    public class Restaurant : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string RestaurantName { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [MaxLength(OwnerNameMaxLength)]
        public string OwnerName { get; set; }

        [RegularExpression(WorkingTimePattern)]
        public string WorkingTime { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        [MaxLength(ContactInfoMaxLength)]
        public virtual string ContactInfo { get; set; }

        [NotMapped]
        public virtual IEnumerable<string> Images { get; set; }
    }
}
