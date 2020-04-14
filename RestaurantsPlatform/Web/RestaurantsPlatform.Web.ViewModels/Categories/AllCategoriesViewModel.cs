namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Mapping;

    public class AllCategoriesViewModel : IHaveCustomMappings
    {
        public IEnumerable<DetailsAllCategoriesViewModel> Categories { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<IQueryable<Category>, AllCategoriesViewModel>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src));
        }
    }
}
