namespace RestaurantsPlatform.Seed
{
    using System;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;

    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}
