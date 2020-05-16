namespace RestaurantsPlatform.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    using RestaurantsPlatform.Web.Infrastructure;

    public class CreateCommentInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string RestaurantName { get; set; }

        [Required]
        public string CommentContent { get; set; }
    }
}
