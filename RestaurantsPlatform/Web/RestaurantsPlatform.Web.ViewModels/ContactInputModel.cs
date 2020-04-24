namespace RestaurantsPlatform.Web.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Web.Infrastructure;

    public class ContactInputModel
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        [DisplayName("Site email")]
        public string EmailTo { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [DisplayName("Message")]
        public string Content { get; set; }

        [GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
