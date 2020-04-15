namespace RestaurantsPlatform.Web.ViewModels.Categories
{
    using System.Collections.Generic;

    public class AllCategoriesViewModel
    {
        public IEnumerable<DetailsAllCategoriesViewModel> Categories { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
