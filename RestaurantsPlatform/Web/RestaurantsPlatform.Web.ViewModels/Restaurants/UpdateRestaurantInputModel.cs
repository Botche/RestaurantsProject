namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Restraurant;

    public class UpdateRestaurantInputModel : IMapFrom<Restaurant>
    {
        [Required]
        public int Id { get; set; }

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
        [Required]
        public string WorkingTime { get; set; }

        [Required]
        [MaxLength(ContactInfoMaxLength)]
        public string ContactInfo { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
