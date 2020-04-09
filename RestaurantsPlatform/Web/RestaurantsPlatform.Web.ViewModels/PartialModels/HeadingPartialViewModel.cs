namespace RestaurantsPlatform.Web.ViewModels.PartialModels
{
    public class HeadingPartialViewModel
    {
        public HeadingPartialViewModel(string name, string routeName, string routeParameterController, int? routeParameterId, string routeParameterAction, string routeParameterName)
        {
            this.Name = name;
            this.RouteName = routeName;
            this.RouteParameterController = routeParameterController;
            this.RouteParameterId = routeParameterId;
            this.RouteParameterAction = routeParameterAction;
            this.RouteParameterName = routeParameterName;
        }

        public string Name { get; set; }

        public string RouteName { get; set; }

        public string RouteParameterController { get; set; }

        public int? RouteParameterId { get; set; }

        public string RouteParameterAction { get; set; }

        public string RouteParameterName { get; set; }
    }
}
