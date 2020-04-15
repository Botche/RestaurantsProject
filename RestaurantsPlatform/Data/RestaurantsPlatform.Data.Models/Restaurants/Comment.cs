namespace RestaurantsPlatform.Data.Models.Restaurants
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using RestaurantsPlatform.Data.Common.Models;

    public class Comment : IDeletableEntity<int>
    {
        public Comment()
        {
            this.Votes = new HashSet<Vote>();
        }

        [Required]
        public string Content { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
