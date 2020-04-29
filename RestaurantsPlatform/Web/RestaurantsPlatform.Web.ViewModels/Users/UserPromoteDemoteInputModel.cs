namespace RestaurantsPlatform.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    public class UserPromoteDemoteInputModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
