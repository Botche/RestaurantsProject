namespace RestaurantsPlatform.Web.Tests.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Data.Repositories;
    using RestaurantsPlatform.Services.Data;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.Areas.Restaurants.Controllers;
    using RestaurantsPlatform.Web.Tests.Mocks;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;
    using RestaurantsPlatform.Web.ViewModels.RestaurantsImages;
    using Xunit;

    using static RestaurantsPlatform.Web.Common.ErrorConstants;

    public class RestaurantImagesControllerTests
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IDeletableEntityRepository<Restaurant> restaurantRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<Vote> voteRepository;
        private readonly IDeletableEntityRepository<Comment> commentRepository;
        private readonly IVoteService voteService;
        private readonly ICommentService commentService;
        private readonly IDeletableEntityRepository<RestaurantImage> restaurantImagesRepository;

        private readonly IRestaurantImageService restaurantImagesService;
        private readonly IUserImageSercice userImageService;
        private readonly ICloudinaryImageService cloudinaryService;
        private readonly IRestaurantService restaurantService;
        private readonly IUserService userService;

        private readonly RestaurantImagesController controller;

        private readonly IConfiguration configuration;

        public RestaurantImagesControllerTests()
        {
            this.dbContext = MockDbContext.GetContext();
            this.configuration = new ConfigurationBuilder()
                    .AddJsonFile("settings.json")
                    .Build();

            this.restaurantImagesRepository = new EfDeletableEntityRepository<RestaurantImage>(this.dbContext);
            this.restaurantRepository = new EfDeletableEntityRepository<Restaurant>(this.dbContext);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.dbContext);
            this.voteRepository = new EfRepository<Vote>(this.dbContext);
            this.commentRepository = new EfDeletableEntityRepository<Comment>(this.dbContext);

            this.voteService = new VoteService(this.voteRepository);
            this.commentService = new CommentService(this.commentRepository, this.voteService);
            this.userImageService = new UserImageSercice(this.userRepository, this.cloudinaryService);
            this.cloudinaryService = new CloudinaryImageService(this.configuration);
            this.restaurantImagesService = new RestaurantImageService(this.restaurantImagesRepository, this.cloudinaryService);
            this.restaurantService = new RestaurantService(this.restaurantRepository, this.restaurantImagesService, this.commentService);
            this.userService = new UserService(this.restaurantService, this.userImageService, this.userRepository);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            this.controller = new RestaurantImagesController(this.restaurantImagesService, this.userService, this.restaurantService)
            {
                TempData = tempData,
            };
        }

        [Fact]
        public void RestaurantImagesController_Constructor()
        {
            Assert.IsType<RestaurantImagesController>(this.controller);
        }

        [Fact]
        public async Task RestaurantImagesController_AddImageToRestaurant_Post()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

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

            var result = await this.controller.AddImageToRestaurant(new AddPictureToRestaurantInputModel
            {
                Id = 1,
                ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                RestaurantName = "Restaurant",
            });

            var viewResult = Assert.IsType<RedirectToRouteResult>(result);

            Assert.Equal("restaurant", viewResult.RouteName);

            var restaurant = this.restaurantRepository.All()
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.Images.FirstOrDefault().ImageUrl,
                })
                .FirstOrDefault(restaurant => restaurant.Id == 1);

            Assert.Contains("https://res.cloudinary.com/djlskbceh/image/upload/", restaurant.ImageUrl);
        }

        [Fact]
        public async Task RestaurantImagesController_Gallery_ZeroImages()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
            });
            await this.restaurantRepository.SaveChangesAsync();

            var result = await this.controller.Gallery(1);

            var model = Assert.IsAssignableFrom<ViewResult>(
                result);
            var returnModel = Assert.IsType<AllImagesFromRestaurantViewModel>(model.Model);

            Assert.Empty(returnModel.Images);
            Assert.Equal(1, returnModel.Id);
            Assert.Equal("Restaurant", returnModel.RestaurantName);
        }

        [Fact]
        public async Task RestaurantImagesController_Gallery_Images()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.restaurantImagesRepository.AddAsync(new RestaurantImage
            {
                Id = 1,
                ImageUrl = "123",
                RestaurantId = 1,
            });
            await this.restaurantImagesRepository.AddAsync(new RestaurantImage
            {
                Id = 2,
                ImageUrl = "123123",
                RestaurantId = 1,
            });
            await this.restaurantImagesRepository.SaveChangesAsync();

            var result = await this.controller.Gallery(1);

            var model = Assert.IsAssignableFrom<ViewResult>(
                result);
            var returnModel = Assert.IsType<AllImagesFromRestaurantViewModel>(model.Model);

            Assert.Equal(2, returnModel.Images.Count());
            Assert.Equal(1, returnModel.Id);
            Assert.Equal("Restaurant", returnModel.RestaurantName);
        }

        [Fact]
        public async Task RestaurantImagesController_Gallery_WrongId()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

            var result = await this.controller.Gallery(1);

            var model = Assert.IsAssignableFrom<ViewResult>(
                result);
            var returnModel = Assert.IsType<ErrorViewModel>(model.Model);

            Assert.Equal(404, returnModel.StatusCode);
            Assert.Equal(PageNotFound, returnModel.Message);
        }

        [Fact]
        public async Task RestaurantImagesController_Update_Get()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
                Images = new List<RestaurantImage>
                {
                    new RestaurantImage
                    {
                        Id = 1,
                        ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                    },
                },
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

            var result = await this.controller.UpdateAsync(1, "https://www.capital.bg/shimg/zx620_3323939.jpg");

            var model = Assert.IsAssignableFrom<ViewResult>(
                result);
            var returnModel = Assert.IsType<UpdateRestaurantImageViewModel>(model.Model);

            Assert.Equal(1, returnModel.Id);
            Assert.Equal("https://www.capital.bg/shimg/zx620_3323939.jpg", returnModel.ImageUrl);
            Assert.Equal("Restaurant", returnModel.RestaurantName);
        }

        [Fact]
        public async Task RestaurantImagesController_Update_Get_Unauthorized()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
                Images = new List<RestaurantImage>
                {
                    new RestaurantImage
                    {
                        Id = 1,
                        ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                    },
                },
            });

            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                    new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                },
                "TestAuthentication"));

            this.controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            var result = await this.controller.UpdateAsync(1, "https://www.capital.bg/shimg/zx620_3323939.jpg");

            var model = Assert.IsAssignableFrom<ViewResult>(
                result);
            var returnModel = Assert.IsType<ErrorViewModel>(model.Model);

            Assert.Equal(401, returnModel.StatusCode);
            Assert.Equal(UnauthorizedMessage, returnModel.Message);
        }

        [Fact]
        public async Task RestaurantImagesController_Update_Get_WrongId()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
                Images = new List<RestaurantImage>
                {
                    new RestaurantImage
                    {
                        Id = 1,
                        ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                    },
                },
            });

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

            var result = await this.controller.UpdateAsync(2, "https://www.capital.bg/shimg/zx620_3323939.jpg");

            var model = Assert.IsAssignableFrom<ViewResult>(
                result);
            var returnModel = Assert.IsType<ErrorViewModel>(model.Model);

            Assert.Equal(401, returnModel.StatusCode);
            Assert.Equal(UnauthorizedMessage, returnModel.Message);
        }

        [Fact]
        public async Task RestaurantImagesController_Update_Post()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

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

            await this.controller.AddImageToRestaurant(new AddPictureToRestaurantInputModel
            {
                Id = 1,
                RestaurantName = "Restaurant",
                ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
            });

            var result = await this.controller.Update(new UpdateRestaurantImageInputModel
            {
                Id = 1,
                ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                OldImageUrl = this.restaurantRepository.All()
                    .FirstOrDefault().Images.FirstOrDefault().ImageUrl,
                RestaurantName = "Restaurant",
            });

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Gallery", viewResult.ActionName);

            var restaurantImageUrl = this.restaurantRepository.All().FirstOrDefault(restaurant => restaurant.Id == 1).Images.FirstOrDefault().ImageUrl;
            Assert.Contains("https://res.cloudinary.com/djlskbceh/image/upload/", restaurantImageUrl);
        }

        [Fact]
        public async Task RestaurantImagesController_Update_Post_WrongModelState()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

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

            this.controller.ModelState.AddModelError("test", "test");
            var result = await this.controller.Update(new UpdateRestaurantImageInputModel
            {
                Id = 1,
                ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                OldImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                RestaurantName = "Restaurant",
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UpdateRestaurantImageInputModel>(viewResult.Model);

            Assert.Equal(1, model.Id);
            Assert.Equal("https://www.capital.bg/shimg/zx620_3323939.jpg", model.ImageUrl);
            Assert.Equal("https://www.capital.bg/shimg/zx620_3323939.jpg", model.OldImageUrl);
            Assert.Equal("Restaurant", model.RestaurantName);
        }

        [Fact]
        public async Task RestaurantImagesController_Update_Post_Unauthorized()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

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
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                    new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                },
                "TestAuthentication"));

            this.controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            var result = await this.controller.Update(new UpdateRestaurantImageInputModel
            {
                Id = 1,
                ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                OldImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                RestaurantName = "Restaurant",
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.Model);

            Assert.Equal(401, model.StatusCode);
            Assert.Equal(UnauthorizedMessage, model.Message);
        }

        [Fact]
        public async Task RestaurantImagesController_Update_Post_WrongId()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

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

            var result = await this.controller.Update(new UpdateRestaurantImageInputModel
            {
                Id = 1,
                ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                OldImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                RestaurantName = "Restaurant",
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.Model);

            Assert.Equal(404, model.StatusCode);
            Assert.Equal(PageNotFound, model.Message);
        }

        [Fact]
        public async Task RestaurantImagesController_Delete()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

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

            await this.controller.AddImageToRestaurant(new AddPictureToRestaurantInputModel
            {
                Id = 1,
                RestaurantName = "Restaurant",
                ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
            });

            var imageUrl = this.restaurantRepository.All()
                .FirstOrDefault()
                .Images
                .FirstOrDefault()
                .ImageUrl;

            var result = await this.controller.Delete(1, imageUrl);

            var model = Assert.IsAssignableFrom<RedirectToActionResult>(
                result);

            Assert.Equal("Gallery", model.ActionName);

            var restaurantImagesCount = this.restaurantRepository.All()
                .FirstOrDefault()
                .Images.Count(image => !image.IsDeleted);

            Assert.Equal(0, restaurantImagesCount);
        }

        [Fact]
        public async Task RestaurantImagesController_Delete_Unauthorized()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
                Images = new List<RestaurantImage>
                {
                    new RestaurantImage
                    {
                        Id = 1,
                        ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                    },
                },
            });

            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "2"),
                    new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                },
                "TestAuthentication"));

            this.controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            var result = await this.controller.Delete(1, "https://www.capital.bg/shimg/zx620_3323939.jpg");

            var model = Assert.IsAssignableFrom<ViewResult>(
                result);
            var returnModel = Assert.IsType<ErrorViewModel>(model.Model);

            Assert.Equal(401, returnModel.StatusCode);
            Assert.Equal(UnauthorizedMessage, returnModel.Message);
        }

        [Fact]
        public async Task RestaurantImagesController_Delete_WrongId()
        {
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);

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

            var result = await this.controller.Delete(1, "https://www.capital.bg/shimg/zx620_3323939.jpg");

            var model = Assert.IsAssignableFrom<ViewResult>(
                result);
            var returnModel = Assert.IsType<ErrorViewModel>(model.Model);

            Assert.Equal(404, returnModel.StatusCode);
            Assert.Equal(PageNotFound, returnModel.Message);
        }
    }
}
