namespace RestaurantsPlatform.Web.Tests.UnitTests
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Data.Repositories;
    using RestaurantsPlatform.Services.Data;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.Areas.Restaurants.Controllers;
    using RestaurantsPlatform.Web.Tests.Mocks;
    using RestaurantsPlatform.Web.ViewModels.Favourites;
    using Xunit;

    public class FavouriteRestaurantsControllerTests
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IDeletableEntityRepository<Restaurant> restaurantRepository;
        private readonly IRepository<FavouriteRestaurant> favouriteRepository;

        private readonly IFavouriteService favouriteService;

        private readonly FavouriteRestaurantsController controller;

        public FavouriteRestaurantsControllerTests()
        {
            this.dbContext = MockDbContext.GetContext();

            this.restaurantRepository = new EfDeletableEntityRepository<Restaurant>(this.dbContext);
            this.favouriteRepository = new EfRepository<FavouriteRestaurant>(this.dbContext);

            this.favouriteService = new FavouriteService(this.favouriteRepository);

            this.controller = new FavouriteRestaurantsController(this.favouriteService);
        }

        [Fact]
        public void FavouriteRestaurantsController_Constructor()
        {
            Assert.IsType<FavouriteRestaurantsController>(this.controller);
        }

        [Fact]
        public async Task FavouriteRestaurantsController_Post_True()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                },
                "TestAuthentication"));

            this.controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            var result = await this.controller.Post(new FavouriteRestaurantsInputModel
            {
                IsFavourite = true,
                RestaurantId = 1,
            });

            var viewResult = Assert.IsType<OkResult>(result);

            var favouriteRestaurant = this.favouriteRepository.All()
                .Select(favourite => new
                {
                    favourite.RestaurantId,
                    favourite.UserId,
                })
                .FirstOrDefault(favourite => favourite.RestaurantId == 1
                                && favourite.UserId == "1");

            Assert.NotNull(favouriteRestaurant);
        }

        [Fact]
        public async Task FavouriteRestaurantsController_Post_False()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                },
                "TestAuthentication"));

            this.controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            await this.controller.Post(new FavouriteRestaurantsInputModel
            {
                IsFavourite = true,
                RestaurantId = 1,
            });
            var result = await this.controller.Post(new FavouriteRestaurantsInputModel
            {
                IsFavourite = false,
                RestaurantId = 1,
            });

            var viewResult = Assert.IsType<OkResult>(result);

            var favouriteRestaurant = this.favouriteRepository.All()
                .Select(favourite => new
                {
                    favourite.RestaurantId,
                    favourite.UserId,
                })
                .FirstOrDefault(favourite => favourite.RestaurantId == 1
                                && favourite.UserId == "1");

            Assert.Null(favouriteRestaurant);
        }

        [Fact]
        public async Task FavouriteRestaurantsController_Post_InvalidModelState()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                },
                "TestAuthentication"));

            this.controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            this.controller.ModelState.AddModelError("error", "error");
            var result = await this.controller.Post(new FavouriteRestaurantsInputModel
            {
                RestaurantId = 1,
            });

            var viewResult = Assert.IsType<BadRequestResult>(result);

            var favouriteRestaurant = this.favouriteRepository.All()
                .Select(favourite => new
                {
                    favourite.RestaurantId,
                    favourite.UserId,
                })
                .FirstOrDefault(favourite => favourite.RestaurantId == 1
                                && favourite.UserId == "1");

            Assert.Null(favouriteRestaurant);
        }
    }
}
