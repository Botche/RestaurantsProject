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
    using RestaurantsPlatform.Web.Controllers;
    using RestaurantsPlatform.Web.Tests.Mocks;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using RestaurantsPlatform.Web.ViewModels.Restaurants;
    using RestaurantsPlatform.Web.ViewModels.RestaurantsImages;
    using Xunit;

    using static RestaurantsPlatform.Web.Infrastructure.ErrorConstants;

    public class RestaurantsControllerTests
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IDeletableEntityRepository<Restaurant> restaurantRepository;
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<FavouriteRestaurant> favouriteRepository;
        private readonly IRepository<Vote> voteRepository;
        private readonly IDeletableEntityRepository<RestaurantImage> restaurantImagesRepository;
        private readonly IDeletableEntityRepository<CategoryImage> categoryImageRepository;

        private readonly IRestaurantImageService restaurantImagesService;
        private readonly IDeletableEntityRepository<Comment> commentRepository;
        private readonly ICloudinaryImageService cloudinaryService;
        private readonly IVoteService voteService;
        private readonly ICommentService commentService;
        private readonly IRestaurantService restaurantService;
        private readonly IUserImageSercice userImageService;
        private readonly ICategoryImageService categoryImageService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly IFavouriteService favouriteService;

        private readonly RestaurantsController controller;

        private readonly IConfiguration configuration;

        public RestaurantsControllerTests()
        {
            this.dbContext = MockDbContext.GetContext();
            this.configuration = new ConfigurationBuilder()
                    .AddJsonFile("settings.json")
                    .Build();

            this.restaurantImagesRepository = new EfDeletableEntityRepository<RestaurantImage>(this.dbContext);
            this.categoryImageRepository = new EfDeletableEntityRepository<CategoryImage>(this.dbContext);
            this.restaurantRepository = new EfDeletableEntityRepository<Restaurant>(this.dbContext);
            this.categoryRepository = new EfDeletableEntityRepository<Category>(this.dbContext);
            this.userRepository = new EfDeletableEntityRepository<ApplicationUser>(this.dbContext);
            this.favouriteRepository = new EfRepository<FavouriteRestaurant>(this.dbContext);
            this.voteRepository = new EfRepository<Vote>(this.dbContext);
            this.commentRepository = new EfDeletableEntityRepository<Comment>(this.dbContext);

            this.cloudinaryService = new CloudinaryImageService(this.configuration);
            this.voteService = new VoteService(this.voteRepository);
            this.commentService = new CommentService(this.commentRepository, this.voteService);
            this.restaurantImagesService = new RestaurantImageService(this.restaurantImagesRepository, this.cloudinaryService);
            this.restaurantService = new RestaurantService(this.restaurantRepository, this.restaurantImagesService, this.commentService);
            this.userImageService = new UserImageSercice(this.userRepository, this.cloudinaryService);

            this.categoryImageService = new CategoryImageService(this.categoryImageRepository, this.cloudinaryService);
            this.categoryService = new CategoryService(this.categoryRepository, this.categoryImageService, this.restaurantService);
            this.userService = new UserService(this.restaurantService, this.userImageService, this.userRepository);
            this.favouriteService = new FavouriteService(this.favouriteRepository);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            this.controller = new RestaurantsController(this.restaurantService, this.categoryService, this.userService, this.configuration, this.favouriteService)
            {
                TempData = tempData,
            };
            AutoMapperConfig.RegisterMappings(typeof(AllImagesFromRestaurantViewModel).Assembly);
        }

        [Fact]
        public void RestaurantsController_Constructor()
        {
            Assert.IsType<RestaurantsController>(this.controller);
        }

        [Fact]
        public async Task RestaurantsController_GetByIdAndName_NoFavourite()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                CategoryId = 1,
                UserId = "1",
                Address = "Address",
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                WorkingTime = "10:00 - 22:00",
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

            var result = this.controller.GetByIdAndName(1, "Restaurant");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DetailsRestaurantViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal("Restaurant", model.RestaurantName);
            Assert.Equal(1, model.Id);
            Assert.Equal(1, model.CategoryId);
            Assert.False(model.IsFavourite);
            Assert.Equal("Category", model.CategoryName);
            Assert.NotNull(viewResult.ViewData["GoogleMapsApiKey"]);
        }

        [Fact]
        public async Task RestaurantsController_GetByIdAndName_Favourite()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                CategoryId = 1,
                UserId = "1",
                Address = "Address",
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                WorkingTime = "10:00 - 22:00",
            });
            await this.restaurantRepository.SaveChangesAsync();

            var user = new ClaimsPrincipal(new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                    new Claim(ClaimTypes.Name, "gunnar@somecompany.com"),
                },
                "TestAuthentication"));

            await this.favouriteRepository.AddAsync(new FavouriteRestaurant
            {
                UserId = "1",
                RestaurantId = 1,
            });
            await this.favouriteRepository.SaveChangesAsync();

            this.controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user },
            };

            var result = this.controller.GetByIdAndName(1, "Restaurant");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DetailsRestaurantViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal("Restaurant", model.RestaurantName);
            Assert.Equal(1, model.Id);
            Assert.Equal(1, model.CategoryId);
            Assert.Equal("Category", model.CategoryName);
            Assert.True(model.IsFavourite);
            Assert.NotNull(viewResult.ViewData["GoogleMapsApiKey"]);
        }

        [Fact]
        public void RestaurantsController_GetByIdAndName_WrongId()
        {
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

            var result = this.controller.GetByIdAndName(2, "Restaurant");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(PageNotFound, model.Message);
            Assert.Equal(404, model.StatusCode);
        }

        [Fact]
        public void RestaurantsController_GetByIdAndName_WrongName()
        {
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

            var result = this.controller.GetByIdAndName(1, "Restaurant123");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(PageNotFound, model.Message);
            Assert.Equal(404, model.StatusCode);
        }

        [Fact]
        public async Task RestaurantsController_Create_Get()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            var result = this.controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(viewResult.ViewData["Categories"]);
            Assert.Single((IEnumerable<AllCategoriesToCreateRestaurantViewModel>)viewResult.ViewData["Categories"]);
        }

        [Fact]
        public void RestaurantsController_Create_Get_WhenCategoriesAreEmpty()
        {
            var result = this.controller.Create();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(404, model.StatusCode);
            Assert.Equal(NoRegisteredCategories, model.Message);
        }

        [Fact]
        public async Task RestaurantsController_Create_Post()
        {
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

            var result = await this.controller.Create(new CreateRestaurantInputModel
            {
                Address = "Address",
                CategoryId = 1,
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                RestaurantName = "Restaurant",
                WorkingTime = "10:00 - 22:00",
            });

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.True(viewResult.RouteValues.TryGetValue("id", out object id));
            Assert.True(viewResult.RouteValues.TryGetValue("name", out object name));

            Assert.Equal(1, id);
            Assert.Equal("restaurant", name);
            Assert.Equal(1, this.restaurantRepository.All().Count());
        }

        [Fact]
        public async Task RestaurantsController_Create_Post_WrongModelState()
        {
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
            var result = await this.controller.Create(new CreateRestaurantInputModel
            {
                Address = "Address",
                CategoryId = 1,
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                RestaurantName = "Restaurant",
                WorkingTime = "10:00 - 22:00",
            });

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<CreateRestaurantInputModel>(viewResult.Model);

            Assert.Equal("Address", model.Address);
            Assert.Equal(1, model.CategoryId);
            Assert.Equal("ContactInfo", model.ContactInfo);
            Assert.Equal("Description", model.Description);
            Assert.Null(model.OwnerName);
            Assert.Equal("Restaurant", model.RestaurantName);
            Assert.Equal("10:00 - 22:00", model.WorkingTime);
        }

        [Fact]
        public async Task RestaurantsController_Update_Get()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                CategoryId = 1,
                UserId = "1",
                Address = "Address",
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                WorkingTime = "10:00 - 22:00",
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

            var result = this.controller.Update(1);

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<UpdateRestaurantViewModel>(viewResult.Model);

            Assert.Equal("Address", model.Address);
            Assert.Equal(1, model.CategoryId);
            Assert.Equal("ContactInfo", model.ContactInfo);
            Assert.Equal("Description", model.Description);
            Assert.Null(model.OwnerName);
            Assert.Equal("Restaurant", model.RestaurantName);
            Assert.Equal("10:00 - 22:00", model.WorkingTime);

            Assert.Single((IEnumerable<AllCategoriesToCreateRestaurantViewModel>)viewResult.ViewData["Categories"]);
        }

        [Fact]
        public async Task RestaurantsController_Update_Get_Unauthorized()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                CategoryId = 1,
                UserId = "1",
                Address = "Address",
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                WorkingTime = "10:00 - 22:00",
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

            var result = this.controller.Update(1);

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(UnauthorizedMessage, model.Message);
            Assert.Equal(401, model.StatusCode);
        }

        [Fact]
        public void RestaurantsController_Update_Get_WrongId()
        {
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

            var result = this.controller.Update(1);

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(PageNotFound, model.Message);
            Assert.Equal(404, model.StatusCode);
        }

        [Fact]
        public async Task RestaurantsController_Update_Post()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant123",
                CategoryId = 1,
                UserId = "1",
                Address = "Address123",
                ContactInfo = "ContactInfo123",
                Description = "Description123",
                OwnerName = null,
                WorkingTime = "10:23 - 22:23",
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

            var result = await this.controller.Update(new UpdateRestaurantInputModel
            {
                Id = 1,
                Address = "Address",
                CategoryId = 1,
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                RestaurantName = "Restaurant",
                WorkingTime = "10:00 - 22:00",
            });

            var viewResult = Assert.IsAssignableFrom<RedirectToRouteResult>(result);

            var model = this.restaurantRepository.All()
                .FirstOrDefault(restaurant => restaurant.Id == 1);
            Assert.Equal("Address", model.Address);
            Assert.Equal(1, model.CategoryId);
            Assert.Equal("ContactInfo", model.ContactInfo);
            Assert.Equal("Description", model.Description);
            Assert.Null(model.OwnerName);
            Assert.Equal("Restaurant", model.RestaurantName);
            Assert.Equal("10:00 - 22:00", model.WorkingTime);

            Assert.Equal("restaurant", viewResult.RouteName);
            Assert.True(viewResult.RouteValues.TryGetValue("id", out object id));
            Assert.True(viewResult.RouteValues.TryGetValue("name", out object name));

            Assert.Equal(1, id);
            Assert.Equal("restaurant", name);
        }

        [Fact]
        public async Task RestaurantsController_Update_Post_WrongId()
        {
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

            var result = await this.controller.Update(new UpdateRestaurantInputModel
            {
                Id = 1,
                Address = "Address",
                CategoryId = 1,
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                RestaurantName = "Restaurant",
                WorkingTime = "10:00 - 22:00",
            });

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(UnauthorizedMessage, model.Message);
            Assert.Equal(401, model.StatusCode);
        }

        [Fact]
        public async Task RestaurantsController_Update_Post_WrongModelState()
        {
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
            var result = await this.controller.Update(new UpdateRestaurantInputModel
            {
                Address = "Address",
                CategoryId = 1,
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                RestaurantName = "Restaurant",
                WorkingTime = "10:00 - 22:00",
            });

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<UpdateRestaurantInputModel>(viewResult.Model);

            Assert.Equal("Address", model.Address);
            Assert.Equal(1, model.CategoryId);
            Assert.Equal("ContactInfo", model.ContactInfo);
            Assert.Equal("Description", model.Description);
            Assert.Null(model.OwnerName);
            Assert.Equal("Restaurant", model.RestaurantName);
            Assert.Equal("10:00 - 22:00", model.WorkingTime);
        }

        [Fact]
        public async Task RestaurantsController_Update_Post_Unauthorized()
        {
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

            var result = await this.controller.Update(new UpdateRestaurantInputModel
            {
                Id = 1,
                Address = "Address",
                CategoryId = 1,
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                RestaurantName = "Restaurant",
                WorkingTime = "10:00 - 22:00",
            });

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(UnauthorizedMessage, model.Message);
            Assert.Equal(401, model.StatusCode);
        }

        [Fact]
        public async Task RestaurantsController_Delete_Get()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                CategoryId = 1,
                UserId = "1",
                Address = "Address",
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                WorkingTime = "10:00 - 22:00",
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

            var result = this.controller.Delete(1);

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<DeleteRestaurantViewModel>(viewResult.Model);

            Assert.Equal("Address", model.Address);
            Assert.Equal("ContactInfo", model.ContactInfo);
            Assert.Equal("Description", model.Description);
            Assert.Null(model.OwnerName);
            Assert.Equal("Restaurant", model.RestaurantName);
            Assert.Equal("10:00 - 22:00", model.WorkingTime);
        }

        [Fact]
        public async Task RestaurantsController_Delete_Get_Unauthorized()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                CategoryId = 1,
                UserId = "1",
                Address = "Address",
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                WorkingTime = "10:00 - 22:00",
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

            var result = this.controller.Delete(1);

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(UnauthorizedMessage, model.Message);
            Assert.Equal(401, model.StatusCode);
        }

        [Fact]
        public void RestaurantsController_Delete_Get_WrongId()
        {
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

            var result = this.controller.Delete(1);

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(PageNotFound, model.Message);
            Assert.Equal(404, model.StatusCode);
        }

        [Fact]
        public async Task RestaurantsController_Delete_Post()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                CategoryId = 1,
                UserId = "1",
                Address = "Address",
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                WorkingTime = "10:00 - 22:00",
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

            var result = await this.controller.Delete(new DeleteRestaurantInputModel
            {
                Id = 1,
            });

            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(result);

            var model = this.restaurantRepository.All()
                .FirstOrDefault(restaurant => restaurant.Id == 1);
            Assert.Null(model);

            Assert.Equal("Categories", viewResult.ControllerName);
            Assert.Equal("All", viewResult.ActionName);
        }

        [Fact]
        public async Task RestaurantsController_Delete_Post_WithImages()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                CategoryId = 1,
                UserId = "1",
                Address = "Address",
                ContactInfo = "ContactInfo",
                Description = "Description",
                OwnerName = null,
                WorkingTime = "10:00 - 22:00",
            });
            await this.restaurantRepository.SaveChangesAsync();

            var imageId = await this.restaurantImagesService
                .AddImageToRestaurantAsync(
                    "https://www.capital.bg/shimg/zx620_3323939.jpg",
                    "Restaurant",
                    1);

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

            var result = await this.controller.Delete(new DeleteRestaurantInputModel
            {
                Id = 1,
            });

            var viewResult = Assert.IsAssignableFrom<RedirectToActionResult>(result);

            var model = this.restaurantRepository.All()
                .FirstOrDefault(restaurant => restaurant.Id == 1);
            Assert.Null(model);

            Assert.Equal(0, this.restaurantImagesRepository.All().Count());

            Assert.Equal("Categories", viewResult.ControllerName);
            Assert.Equal("All", viewResult.ActionName);
        }

        [Fact]
        public async Task RestaurantsController_Delete_Post_WrongModelState()
        {
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
            var result = await this.controller.Delete(new DeleteRestaurantInputModel
            {
                Id = 1,
            });

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<DeleteRestaurantInputModel>(viewResult.Model);

            Assert.Equal(1, model.Id);
        }

        [Fact]
        public async Task RestaurantsController_Delete_Post_WrongId()
        {
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

            var result = await this.controller.Delete(new DeleteRestaurantInputModel
            {
                Id = 1,
            });

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(401, model.StatusCode);
            Assert.Equal(UnauthorizedMessage, model.Message);
        }

        [Fact]
        public async Task RestaurantsController_Delete_Post_Unauthorized()
        {
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

            var result = await this.controller.Delete(new DeleteRestaurantInputModel
            {
                Id = 1,
            });

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

            Assert.Equal(UnauthorizedMessage, model.Message);
            Assert.Equal(401, model.StatusCode);
        }
    }
}
