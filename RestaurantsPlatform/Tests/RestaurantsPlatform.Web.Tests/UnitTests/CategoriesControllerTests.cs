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
    using RestaurantsPlatform.Web.Areas.Categories.Controllers;
    using RestaurantsPlatform.Web.Tests.Mocks;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Categories;
    using Xunit;

    using static RestaurantsPlatform.Web.Infrastructure.ErrorConstants;

    public class CategoriesControllerTests
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IDeletableEntityRepository<Restaurant> restaurantRepository;
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IRepository<FavouriteRestaurant> favouriteRepository;
        private readonly IDeletableEntityRepository<RestaurantImage> restaurantImagesRepository;
        private readonly IDeletableEntityRepository<CategoryImage> categoryImageRepository;

        private readonly IRestaurantImageService restaurantImagesService;
        private readonly ICloudinaryImageService cloudinaryService;
        private readonly IRestaurantService restaurantService;
        private readonly ICategoryImageService categoryImageService;
        private readonly ICategoryService categoryService;
        private readonly IUserService userService;
        private readonly IFavouriteService favouriteService;

        private readonly CategoriesController controller;

        private readonly IConfiguration configuration;

        public CategoriesControllerTests()
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

            this.cloudinaryService = new CloudinaryImageService(this.configuration);
            this.restaurantImagesService = new RestaurantImageService(this.restaurantImagesRepository, this.cloudinaryService);
            this.restaurantService = new RestaurantService(this.restaurantRepository, this.restaurantImagesService);

            this.categoryImageService = new CategoryImageService(this.categoryImageRepository, this.cloudinaryService);
            this.categoryService = new CategoryService(this.categoryRepository, this.categoryImageService, this.restaurantService);
            this.userService = new UserService(this.restaurantService, this.userRepository);
            this.favouriteService = new FavouriteService(this.favouriteRepository);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            this.controller = new CategoriesController(this.categoryService, this.restaurantService, this.favouriteService)
            {
                TempData = tempData,
            };
        }

        [Fact]
        public void CategoriesController_Constructor()
        {
            Assert.IsType<CategoriesController>(this.controller);
        }

        [Fact]
        public async Task CategoriesController_All()
        {
            var expectedPagesCount = 1;
            var expectedPage = 1;
            var expectedCount = 2;
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category" + 1,
                Image = new CategoryImage { },
            });
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 2,
                Name = "Category" + 2,
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(AllCategoriesViewModel).Assembly);

            var result = this.controller.All();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AllCategoriesViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(expectedCount, model.Categories.Count());
            Assert.Equal(expectedPage, model.CurrentPage);
            Assert.Equal(expectedPagesCount, model.PagesCount);
        }

        [Fact]
        public async Task CategoriesController_AllWithSecondPage()
        {
            var expectedCount = 1;
            var expectedPage = 2;
            var expectedPagesCount = 2;
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 2,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 3,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 4,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(AllCategoriesViewModel).Assembly);

            var result = this.controller.All(2);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AllCategoriesViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(expectedCount, model.Categories.Count());
            Assert.Equal(expectedPage, model.CurrentPage);
            Assert.Equal(expectedPagesCount, model.PagesCount);
        }

        [Fact]
        public void CategoriesController_All_WithEmptyList()
        {
            var expectedPagesCount = 1;
            var expectedPage = 1;
            var expectedCount = 0;

            AutoMapperConfig.RegisterMappings(typeof(AllCategoriesViewModel).Assembly);

            var result = this.controller.All();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AllCategoriesViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(expectedCount, model.Categories.Count());
            Assert.Equal(expectedPage, model.CurrentPage);
            Assert.Equal(expectedPagesCount, model.PagesCount);
        }

        [Fact]
        public void CategoriesController_All_WithWrongPage()
        {
            var expectedPagesCount = 1;
            var expectedPage = 1;
            var expectedCount = 0;

            AutoMapperConfig.RegisterMappings(typeof(AllCategoriesViewModel).Assembly);

            var result = this.controller.All(-2);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<AllCategoriesViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(expectedCount, model.Categories.Count());
            Assert.Equal(expectedPage, model.CurrentPage);
            Assert.Equal(expectedPagesCount, model.PagesCount);
        }

        [Fact]
        public async Task CategoriesController_GetByIdAndName()
        {
            var expectedId = 1;
            var expectedName = "Category";
            await this.categoryRepository.AddAsync(new Category
            {
                Id = expectedId,
                Name = expectedName,
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(DetailsCategoryViewModel).Assembly);

            var result = this.controller.GetByIdAndName(expectedId, expectedName);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DetailsCategoryViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
        }

        [Fact]
        public async Task CategoriesController_GetByIdAndName_WihtRestaurants()
        {
            var expectedId = 1;
            var expectedName = "Category";

            await this.userRepository.AddAsync(new ApplicationUser
            {
                Id = "1",
                UserName = "Pesho",
                FavouriteRestaurants = new List<FavouriteRestaurant>
                {
                    new FavouriteRestaurant
                    {
                        Id = "1",
                        UserId = "1",
                        RestaurantId = 1,
                    },
                },
            });
            await this.categoryRepository.AddAsync(new Category
            {
                Id = expectedId,
                Name = expectedName,
                Image = new CategoryImage { },
                Restaurants = new List<Restaurant>
                {
                    new Restaurant
                    {
                        Id = 1,
                        RestaurantName = "Name",
                        UserId = "1",
                    },
                    new Restaurant
                    {
                        Id = 2,
                        RestaurantName = "Name",
                        UserId = "1",
                    },
                },
            });
            await this.categoryRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(DetailsCategoryViewModel).Assembly);

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

            var result = this.controller.GetByIdAndName(expectedId, expectedName);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DetailsCategoryViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Restaurants.Count());
            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
        }

        [Fact]
        public async Task CategoriesController_GetByIdAndName_WithWrongPage()
        {
            var expectedPagesCount = 1;
            var expectedPage = 1;
            var expectedCount = 0;
            var expectedId = 1;
            var expectedName = "Category";

            await this.categoryRepository.AddAsync(new Category
            {
                Id = expectedId,
                Name = expectedName,
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(DetailsCategoryViewModel).Assembly);

            var result = this.controller.GetByIdAndName(expectedId, expectedName, -2);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<DetailsCategoryViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(expectedId, model.Id);
            Assert.Equal(expectedName, model.Name);
            Assert.Equal(expectedCount, model.Restaurants.Count());
            Assert.Equal(expectedPage, model.CurrentPage);
            Assert.Equal(expectedPagesCount, model.PagesCount);
        }

        [Fact]
        public void CategoriesController_GetByIdAndName_WithWrongCategory()
        {
            var result = this.controller.GetByIdAndName(1, "Category");

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(PageNotFound, model.Message);
            Assert.Equal(404, model.StatusCode);
        }

        [Fact]
        public void CategoriesController_Create_Get()
        {
            var result = this.controller.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CategoriesController_Create_Post()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(DetailsCategoryViewModel).Assembly);

            var result = await this.controller.Create(new CreateCategoryInputModel
            {
                Name = "Category",
                Description = "Category",
                ImageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg",
                Title = "Category",
            });

            var model = Assert.IsAssignableFrom<RedirectToRouteResult>(
                result);

            var expected = 2;
            var actual = this.categoryRepository.All().Count();
            Assert.Equal(expected, actual);
            Assert.Equal("category", model.RouteName);
        }

        [Fact]
        public async Task CategoriesController_Create_Post_WrongModelState()
        {
            AutoMapperConfig.RegisterMappings(typeof(CreateCategoryInputModel).Assembly);

            this.controller.ModelState.AddModelError("test", "test");
            var result = await this.controller.Create(new CreateCategoryInputModel()
            {
                Name = "Pesho",
            });

            var model = Assert.IsAssignableFrom<IActionResult>(
                result);
            var inputModel = Assert.IsType<ViewResult>(model);
            var viewResultModel = Assert.IsType<CreateCategoryInputModel>(inputModel.Model);

            var categoryCount = this.categoryRepository.All().Count();

            Assert.Equal(0, categoryCount);
            Assert.Equal("Pesho", viewResultModel.Name);
            Assert.Null(viewResultModel.Description);
            Assert.Null(viewResultModel.Title);
        }

        [Fact]
        public async Task CategoriesController_Update_Get()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(UpdateCategoryViewModel).Assembly);

            var result = this.controller.Update(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CategoriesController_Update_Post()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage
                {
                    ImageUrl = "123",
                },
            });
            await this.categoryRepository.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(DetailsCategoryViewModel).Assembly);

            var result = await this.controller.Update(new UpdateCategoryInputModel
            {
                Name = "Category1",
                Description = "Category1",
                Title = "Category1",
                Id = 1,
            });

            var model = Assert.IsAssignableFrom<RedirectToRouteResult>(
                result);

            Assert.Equal("category", model.RouteName);

            var category = this.categoryRepository.All()
                .FirstOrDefault(restaurant => restaurant.Id == 1);

            Assert.Equal(1, category.Id);
            Assert.Equal("Category1", category.Name);
            Assert.Equal("Category1", category.Description);
            Assert.Equal("Category1", category.Title);
            Assert.Equal("123", category.Image.ImageUrl);
        }

        [Fact]
        public async Task CategoriesController_Update_Post_WrongModelState()
        {
            AutoMapperConfig.RegisterMappings(typeof(UpdateCategoryInputModel).Assembly);

            this.controller.ModelState.AddModelError("test", "test");
            var result = await this.controller.Update(new UpdateCategoryInputModel
            {
                Id = 1,
            });

            var model = Assert.IsAssignableFrom<IActionResult>(
                result);
            var inputModel = Assert.IsType<ViewResult>(model);
            var viewResultModel = Assert.IsType<UpdateCategoryInputModel>(inputModel.Model);

            var categoryCount = this.categoryRepository.All().Count();

            Assert.Equal(0, categoryCount);
            Assert.Equal(1, viewResultModel.Id);
            Assert.Null(viewResultModel.Name);
            Assert.Null(viewResultModel.Description);
            Assert.Null(viewResultModel.Title);
        }

        [Fact]
        public async Task CategoriesController_Delete_Get()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage { },
            });
            await this.categoryRepository.SaveChangesAsync();

            var result = this.controller.Delete(1);

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task CategoriesController_Delete_Post()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage
                {
                    PublicId = "1",
                    ImageUrl = "123",
                },
            });
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 2,
                Name = "Category",
                Image = new CategoryImage
                {
                    PublicId = "1",
                    ImageUrl = "123",
                },
            });
            await this.categoryRepository.SaveChangesAsync();

            var result = await this.controller.Delete(new DeleteCategoryInputModel
            {
                Id = 1,
            });

            var action = Assert.IsAssignableFrom<RedirectToActionResult>(result);

            var expected = 1;
            var actual = this.categoryRepository.All().Count();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task CategoriesController_Delete_Post_WrongModelState()
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                Image = new CategoryImage
                {
                    PublicId = "1",
                    ImageUrl = "123",
                },
            });
            await this.categoryRepository.SaveChangesAsync();
            AutoMapperConfig.RegisterMappings(typeof(UpdateCategoryInputModel).Assembly);

            this.controller.ModelState.AddModelError("test", "test");
            var result = await this.controller.Delete(new DeleteCategoryInputModel
            {
                Id = 2,
            });

            var model = Assert.IsAssignableFrom<IActionResult>(
                result);
            var inputModel = Assert.IsType<ViewResult>(model);
            var viewResultModel = Assert.IsType<DeleteCategoryInputModel>(inputModel.Model);

            var categoryCount = this.categoryRepository.All().Count();

            Assert.Equal(1, categoryCount);
            Assert.Equal(2, viewResultModel.Id);
        }
    }
}
