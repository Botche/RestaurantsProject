namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface IUserService
    {
        bool CheckIfCurrentUserIsAuthor(string authorId, string currentUserId);

        bool CheckIfCurrentUserIsAuthorByGivenId(int restaurantId, string currentUserId);
    }
}
