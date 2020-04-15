namespace RestaurantsPlatform.Web.ViewModels.Votes
{
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class DetailsVotesViewModel : IMapFrom<Vote>
    {
        public string UserId { get; set; }

        public VoteType Type { get; set; }
    }
}
