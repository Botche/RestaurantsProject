namespace RestaurantsPlatform.Web.ViewModels.Votes
{
    using System.ComponentModel.DataAnnotations;

    public class VoteInputModel
    {
        [Required]
        public string CommentId { get; set; }

        [Required]
        public bool IsUpVote { get; set; }
    }
}
