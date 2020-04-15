namespace RestaurantsPlatform.Web.ViewModels.Comments
{
    using System;

    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class DetailsCommentViewModel : IMapFrom<Comment>
    {
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorId { get; set; }

        public DateTime ModifiedOn { get; set; }

        public bool IsEdited => this.ModifiedOn.Year > 2000;
    }
}
