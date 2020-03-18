namespace Sandbox
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    using CommandLine;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using RestaurantsProject.Data;
    using RestaurantsProject.Data.Common;
    using RestaurantsProject.Data.Common.Repositories;
    using RestaurantsProject.Data.Models;
    using RestaurantsProject.Data.Repositories;
    using RestaurantsProject.Seed.Seeding;
    using RestaurantsProject.Services.Messaging;
    using RestaurantsProject.Web;

    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
