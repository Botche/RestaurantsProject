namespace RestaurantsPlatform.Web.ViewModels.Restaurants
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.Infrastructure;

    using static RestaurantsPlatform.Data.Common.Constants.Models.Restraurant;

    public class UpdateRestaurantViewModel : IMapFrom<Restaurant>
    {
        public int Id { get; set; }

        [DisplayName("Restaurant Name")]
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string RestaurantName { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [DisplayName("Owner Name")]
        [MaxLength(OwnerNameMaxLength)]
        public string OwnerName { get; set; }

        [DisplayName("Working Time")]
        [RegularExpression(WorkingTimePattern)]
        [Required]
        public string WorkingTime { get; set; }

        [DisplayName("Contact Info")]
        [Required]
        [MaxLength(ContactInfoMaxLength)]
        public string ContactInfo { get; set; }

        [DisplayName("Category")]
        [Required]
        public int CategoryId { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }

        public string UserId { get; set; }

        public string Url => this.RestaurantName.Replace(' ', '-').ToLower();
    }
}
