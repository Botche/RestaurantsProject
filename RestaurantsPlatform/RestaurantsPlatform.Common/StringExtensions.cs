namespace RestaurantsPlatform.Common
{
    public static class StringExtensions
    {
        public static string ToSlug(this string str)
        {
            return str.ToLower().Replace(" ", "-");
        }
    }
}
