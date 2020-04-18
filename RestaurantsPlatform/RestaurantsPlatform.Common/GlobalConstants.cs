namespace RestaurantsPlatform.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Restaurants Platform";

        public const string OwnersEmail = "restaurant_platform@abv.bg";
        public const string OwnersName = "Restaurants Platform";

        public const string UserRoleName = "User";
        public const string UserEmail = "user@user.user";
        public const string UserUsername = "user";
        public const string UserPassword = "user@user.user";

        public const string RestaurantRoleName = "RestaurantOwner";
        public const string RestaurantEmail = "owner@owner.owner";
        public const string RestaurantUsername = "owner";
        public const string RestaurantPassword = "owner@owner.owner";
        public const string SecondRestaurantEmail = "test@test.test";
        public const string SecondRestaurantUsername = "test";
        public const string SecondRestaurantPassword = "test@test.test";

        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorEmail = "admin@admin.admin";
        public const string AdministratorUsername = "admin";
        public const string AdministratorPassword = "admin@admin.admin";

        public const string AdministratorOrRestaurantOwner = AdministratorRoleName + "," + RestaurantRoleName;
    }
}
