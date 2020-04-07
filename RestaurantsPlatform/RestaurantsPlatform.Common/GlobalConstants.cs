namespace RestaurantsPlatform.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Restaurants Platform";

        public const string UserRoleName = "User";
        public const string UserEmail = "user@user.user";
        public const string UserPassword = "user@user.user";

        public const string RestaurantRoleName = "RestaurantOwner";
        public const string RestaurantEmail = "restaurantOwner@restaurantOwner.restaurantOwner";
        public const string RestaurantPassword = "restaurantOwner@restaurantOwner.restaurantOwner";
        public const string SecondRestaurantEmail = "test@test.test";
        public const string SecondRestaurantPassword = "test@test.test";

        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorEmail = "administrator@administrator.administrator";
        public const string AdministratorPassword = "administrator@administrator.administrator";

        public const string AdministratorOrRestaurantOwner = AdministratorRoleName + "," + RestaurantRoleName;
    }
}
