namespace RestaurantsPlatform.Web.ViewModels.RestaurantsImages
{
    public class UpdateRestaurantImageInputModel
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string OldImageUrl { get; set; }

        public string RestaurantName { get; set; }
    }
}
