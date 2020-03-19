namespace RestaurantsPlatform.Data.Common.Constants.Models
{
    public static class Restraurant
    {
        public const int NameMaxLength = 64;

        public const int DescriptionMaxLength = 512;

        public const int AddressMaxLength = 256;

        public const int OwnerNameMaxLength = 64;

        public const string WorkingTimePattern = @"[\d]{2}:[\d]{2} - [\d]{2}:[\d]{2}";

        public const int ContactInfoMaxLength = 128;
    }
}
