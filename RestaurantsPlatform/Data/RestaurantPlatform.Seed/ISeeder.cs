// <copyright file="ISeeder.cs" company="RestaurantsPlatform">
// Copyright (c) RestaurantsPlatform. All Rights Reserved.
// </copyright>

namespace RestaurantsPlatform.Seed
{
    using System;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;

    /// <summary>
    /// Seeder interface.
    /// </summary>
    public interface ISeeder
    {
        Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider);
    }
}
