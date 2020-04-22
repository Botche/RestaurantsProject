// <copyright file="ApplicationDbContextSeeder.cs" company="RestaurantsPlatform">
// Copyright (c) RestaurantsPlatform. All Rights Reserved.
// </copyright>

namespace RestaurantsPlatform.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Seed.Seeding;
    using RestaurantsPlatform.Services.Data;
    using RestaurantsPlatform.Services.Data.Interfaces;

    /// <summary>
    /// Database used fo seeding.
    /// </summary>
    public class ApplicationDbContextSeeder : ISeeder
    {
        private readonly IConfiguration configuration;
        private readonly ICloudinaryImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContextSeeder"/> class.
        /// </summary>
        /// <param name="configuration">appsettings.json.</param>
        public ApplicationDbContextSeeder(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.imageService = new CloudinaryImageService(this.configuration);
        }

        /// <summary>
        /// Method seeding infomation into database and logs information.
        /// </summary>
        /// <param name="dbContext">Database to seed in.</param>
        /// <param name="serviceProvider">Service provider for logging.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new UsersSeeder(),
                              new CategoryImagesSeeder(this.imageService),
                              new CategoriesSeeder(),
                              new RestaurantsSeeder(),
                              new RestaurantImagesSeeder(this.imageService),
                              new FavouritesSeeder(),
                              new CommentsSeeder(),
                              new VotesSeeder(),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
