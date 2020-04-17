namespace RestaurantsPlatform.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateCommentInputView
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
