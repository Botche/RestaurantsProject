namespace RestaurantsPlatform.Web.ViewModels.Comments
{
    using System.ComponentModel.DataAnnotations;

    public class DeleteCommentInputModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int CommentId { get; set; }
    }
}
