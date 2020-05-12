namespace RestaurantsPlatform.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AutoMapper;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;

    public class DetailsUserViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public int VotesCount { get; set; }

        public int RestaurantsCount { get; set; }

        public int CommentsCount { get; set; }

        public int FavouriteRestaurantsCount { get; set; }

        public IEnumerable<AllRestaurantsViewModel> FavouriteRestaurants { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, DetailsUserViewModel>()
                .ForMember(
                    dest => dest.VotesCount,
                    ori => ori.MapFrom(from => from.Votes
                        .Count(vote => vote.Type != VoteType.Neutral && !vote.Comment.IsDeleted)))
                .ForMember(
                    dest => dest.CommentsCount,
                    ori => ori.MapFrom(from => from.Comments
                        .Count(comment => !comment.IsDeleted)))
                .ForMember(
                    dest => dest.RestaurantsCount,
                    ori => ori.MapFrom(from => from.Restaurants
                        .Count(restaurant => !restaurant.IsDeleted)))
                .ForMember(
                    dest => dest.FavouriteRestaurantsCount,
                    ori => ori.MapFrom(from => from.FavouriteRestaurants
                        .Count(favourite => !favourite.Restaurant.IsDeleted)))
                .ForMember(
                    dest => dest.RoleName,
                    ori => ori.MapFrom(from => from.UserRoles.FirstOrDefault()
                                                                .Role.Name))
                .ForMember(
                    dest => dest.FavouriteRestaurants,
                    ori => ori.MapFrom(from => from.FavouriteRestaurants.
                                                    Select(favourite => favourite.Restaurant)
                                                    .Where(favourite => !favourite.IsDeleted)));
        }
    }
}
