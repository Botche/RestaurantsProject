namespace RestaurantsPlatform.Web.ViewModels.Comments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.Votes;

    public class DetailsCommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorId { get; set; }

        public string AuthorImageUrl { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsEdited => this.ModifiedOn.Year > 2000;

        public int VotesSum => this.Votes.Sum(vote => (int)vote.Type);

        public IEnumerable<DetailsVotesViewModel> Votes { get; set; }
    }
}
