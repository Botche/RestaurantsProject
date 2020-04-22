namespace RestaurantsPlatform.Web.ViewModels.Users
{
    using System;
    using System.Linq;

    using AutoMapper;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class DetailsUserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public int VotesCount { get; set; }

        public int RestaurantsCount { get; set; }

        public int CommentsCount { get; set; }

        public int FavouriteRestaurantsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, DetailsUserViewModel>()
                .ForMember(
                    dest => dest.VotesCount,
                    ori => ori.MapFrom(from => from.Votes
                        .Count(vote => vote.Type != VoteType.Neutral)));
        }
    }
}
