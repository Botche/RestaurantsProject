namespace RestaurantsPlatform.Web.Common
{
    public static class EmailSentInformationConstants
    {
        public const string DefaultSenderName = "Unknown";

        public const string PromotedEmailSubject = "Congrationations! You have been promoted!";
        public const string PromotedEmailHtmlContent = "<h1>Congrats you have been promoted in our web site - Restaurants Platform!</h1>";

        public const string DemotedEmailSubject = "You have been demoted!";
        public const string DemotedEmailHtmlContent = "<h1>You have been demoted in our web site - Restaurants Platform!</h1>";

        public const string BannedEmailSubject = "We are sorry!";
        public const string BannedEmailHtmlContent = @"<h1>You have been banned from our web site - Restaurants Platform...</h1><p>If you want your account back, contact us!</p>";

        public const string UnBannedEmailSubject = "We are happy to see you again!";
        public const string UnBannedEmailHtmlContent = @"<h1>You have been unbanned from our web site - Restaurants Platform :)</h1>";
    }
}
