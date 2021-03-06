﻿namespace RestaurantsPlatform.Web
{
    using System;
    using System.Reflection;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Common;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Data.Repositories;
    using RestaurantsPlatform.Seed;
    using RestaurantsPlatform.Services.Data;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Services.Messaging;
    using RestaurantsPlatform.Web.ViewModels;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddResponseCaching();
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddRazorPages();
            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = this.configuration.GetSection("Facebook")["AppId"];
                facebookOptions.AppSecret = this.configuration.GetSection("Facebook")["AppSecret"];
                facebookOptions.AccessDeniedPath = "/Identity/Account/Login";
            });

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            this.RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder(this.configuration)
                    .SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
                app.Use(async (context, next) =>
                {
                    await next();
                    if (context.Response.StatusCode == 404)
                    {
                        context.Request.Path = "/Home/Error";
                        await next();
                    }
                });
            }

            app.UseResponseCompression();
            app.UseResponseCaching();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "restaurantComment",
                        pattern: "r/{id:int}/{name:minlength(3)}/comment/{commentId:int}/{action}",
                        defaults: new { area = "Restaurants", controller = "Comments" });

                    endpoints.MapControllerRoute(
                        name: "restaurant",
                        pattern: "r/{id:int}/{name:minlength(3)}",
                        defaults: new { area = "Restaurants", controller = "Restaurants", action = "GetByIdAndName" });
                    endpoints.MapControllerRoute(
                        name: "restaurantWithAction",
                        pattern: "r/{id:int}/{name:minlength(3)}/{action}",
                        defaults: new { area = "Restaurants", controller = "Restaurants" });
                    endpoints.MapControllerRoute(
                        name: "restaurantImages",
                        pattern: "r/{id:int}/{name:minlength(3)}/{action}/images",
                        defaults: new { area = "Restaurants", controller = "RestaurantImages" });

                    endpoints.MapControllerRoute(
                        name: "categoryImage",
                        pattern: "c/{id:int}/{name:minlength(3)}/{action}/image",
                        defaults: new { area = "Categories", controller = "CategoryImages" });
                    endpoints.MapControllerRoute(
                        name: "categoryWithAction",
                        pattern: "c/{id:int}/{name:minlength(3)}/{action}",
                        defaults: new { area = "Categories", controller = "Categories" });
                    endpoints.MapControllerRoute(
                        name: "category",
                        pattern: "c/{id:int}/{name:minlength(3)}",
                        defaults: new { area = "Categories", controller = "Categories", action = "GetByIdAndName" });

                    endpoints.MapControllerRoute(
                        name: "categoryArea",
                        pattern: "{controller}/{action}/{id?}",
                        defaults: new { area = "Categories" });

                    endpoints.MapControllerRoute(
                        name: "restaurantArea",
                        pattern: "{controller}/{action}/{id?}",
                        defaults: new { area = "Restaurants" });

                    endpoints.MapControllerRoute(
                        name: "areaRoute",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");
                    endpoints.MapRazorPages();
                });
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender>(options =>
                            new SendGridEmailSender(this.configuration.GetSection("SendGrid")["ApiKey"]));
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICloudinaryImageService, CloudinaryImageService>();
            services.AddTransient<IRestaurantImageService, RestaurantImageService>();
            services.AddTransient<ICategoryImageService, CategoryImageService>();
            services.AddTransient<IAdministrationService, AdministrationService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IVoteService, VoteService>();
            services.AddTransient<IFavouriteService, FavouriteService>();
            services.AddTransient<IUserImageSercice, UserImageService>();
        }
    }
}
