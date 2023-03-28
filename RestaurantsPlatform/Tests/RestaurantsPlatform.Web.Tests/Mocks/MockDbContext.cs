namespace RestaurantsPlatform.Web.Tests.Mocks
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using RestaurantsPlatform.Data;

    public static class MockDbContext
    {
        public static ApplicationDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            return new ApplicationDbContext(options);
        }
    }
}
