namespace RestaurantsPlatform.Data.Models.Restaurants
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using RestaurantsPlatform.Data.Common.Models;

    public class Vote : BaseModel<int>
    {

        [Required]
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public VoteType Type { get; set; }
    }
}
