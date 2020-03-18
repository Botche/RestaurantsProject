namespace RestaurantsProject.Seed.Seeding
{
    using System;
    using System.Threading.Tasks;

    using RestaurantsProject.Data;

    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}
