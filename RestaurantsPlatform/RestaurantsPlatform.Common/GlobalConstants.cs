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
        public const string UserPassword = "Password123";

        public const string RestaurantRoleName = "RestaurantOwner";
        public const string RestaurantEmail = "owner@owner.owner";
        public const string RestaurantUsername = "owner";
        public const string RestaurantPassword = "Password123";
        public const string SecondRestaurantEmail = "test@test.test";
        public const string SecondRestaurantUsername = "test";
        public const string SecondRestaurantPassword = "Password123";

        public const string AdministratorRoleName = "Administrator";
        public const string AdministratorEmail = "admin@admin.admin";
        public const string AdministratorUsername = "admin";
        public const string AdministratorPassword = "Password123";

        public const string RealUserEmailOne = "gabrielpetkov622@gmail.com";
        public const string RealUserUsernameOne = "Botche";
        public const string RealUserPasswordOne = "Password123";
        public const string RealUserEmailTwo = "new.2000@abv.bg";
        public const string RealUserUsernameTwo = "Tester";
        public const string RealUserPasswordTwo = "Password123";

        public const string AdministratorOrRestaurantOwner = AdministratorRoleName + "," + RestaurantRoleName;
    }
}
