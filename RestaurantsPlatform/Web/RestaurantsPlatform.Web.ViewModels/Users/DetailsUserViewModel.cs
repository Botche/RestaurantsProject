namespace RestaurantsPlatform.Web.ViewModels.Users
{
    using System;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Mapping;

    public class DetailsUserViewModel : IMapFrom<ApplicationUser>
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int VotesCount { get; set; }

        public int RestaurantsCount { get; set; }

        public int CommentsCount { get; set; }

        public int FavouriteRestaurantsCount { get; set; }
    }
}
