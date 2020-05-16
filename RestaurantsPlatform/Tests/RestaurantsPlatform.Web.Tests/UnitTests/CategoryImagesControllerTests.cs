namespace RestaurantsPlatform.Web.Tests.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.Extensions.Configuration;
    using Moq;
    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Data.Repositories;
    using RestaurantsPlatform.Services.Data;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.Areas.Categories.Controllers;
    using RestaurantsPlatform.Web.Tests.Mocks;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;
    using Xunit;

    public class CategoryImagesControllerTests
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IDeletableEntityRepository<Restaurant> restaurantRepository;
        private readonly IDeletableEntityRepository<Category> categoryRepository;
        private readonly IRepository<Vote> voteRepository;
        private readonly IDeletableEntityRepository<Comment> commentRepository;
        private readonly IVoteService voteService;
        private readonly ICommentService commentService;
        private readonly IDeletableEntityRepository<RestaurantImage> restaurantImagesRepository;
        private readonly IDeletableEntityRepository<CategoryImage> categoryImageRepository;

        private readonly IRestaurantImageService restaurantImagesService;
        private readonly ICloudinaryImageService cloudinaryService;
        private readonly IRestaurantService restaurantService;
        private readonly ICategoryImageService categoryImageService;
        private readonly ICategoryService categoryService;

        private readonly CategoryImagesController controller;

        private readonly IConfiguration configuration;

        public CategoryImagesControllerTests()
        {
            this.dbContext = MockDbContext.GetContext();
            this.configuration = new ConfigurationBuilder()
                    .AddJsonFile("settings.json")
                    .Build();

            this.restaurantImagesRepository = new EfDeletableEntityRepository<RestaurantImage>(this.dbContext);
            this.categoryImageRepository = new EfDeletableEntityRepository<CategoryImage>(this.dbContext);
            this.restaurantRepository = new EfDeletableEntityRepository<Restaurant>(this.dbContext);
            this.categoryRepository = new EfDeletableEntityRepository<Category>(this.dbContext);
            this.voteRepository = new EfRepository<Vote>(this.dbContext);
            this.commentRepository = new EfDeletableEntityRepository<Comment>(this.dbContext);

            this.voteService = new VoteService(this.voteRepository);
            this.commentService = new CommentService(this.commentRepository, this.voteService);
            this.cloudinaryService = new CloudinaryImageService(this.configuration);
            this.restaurantImagesService = new RestaurantImageService(this.restaurantImagesRepository, this.cloudinaryService);
            this.restaurantService = new RestaurantService(this.restaurantRepository, this.restaurantImagesService, this.commentService);

            this.categoryImageService = new CategoryImageService(this.categoryImageRepository, this.cloudinaryService);
            this.categoryService = new CategoryService(this.categoryRepository, this.categoryImageService, this.restaurantService);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            this.controller = new CategoryImagesController(this.categoryService)
            {
                TempData = tempData,
            };
        }

        [Fact]
        public void CategoryImagesController_Constructor()
        {
            Assert.IsType<CategoryImagesController>(this.controller);
        }

        [Fact]
        public void CategoryImagesController_Update_Get()
        {
            int categoryId = 1;
            string imageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg";

            var result = this.controller.Update(categoryId, imageUrl);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UpdateCategoryImageBindingModel>(
                viewResult.ViewData.Model);

            Assert.Equal(categoryId, model.CategoryId);
            Assert.Equal(imageUrl, model.ImageUrl);
        }

        [Fact]
        public async Task CategoryImagesController_Update_Post()
        {
            var imageId = await this.categoryImageService.AddImageToCategoryAsync("https://www.capital.bg/shimg/zx620_3323939.jpg", "Category");

            await this.categoryRepository.AddAsync(new Category
            {
                Id = 1,
                Name = "Category",
                ImageId = imageId,
            });
            await this.categoryRepository.SaveChangesAsync();

            int categoryId = 1;
            string imageUrl = "https://www.capital.bg/shimg/zx620_3323939.jpg";

            AutoMapperConfig.RegisterMappings(typeof(PublicIdCategoryImageBindinModel).Assembly);
            var result = await this.controller.Update(new UpdateCategoryImageBindingModel
            {
                CategoryId = categoryId,
                ImageUrl = imageUrl,
            });

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("All", viewResult.ActionName);
            Assert.Equal("Categories", viewResult.ControllerName);

            var categoryImageUrl = this.categoryRepository.All().FirstOrDefault(category => category.Id == categoryId).Image.ImageUrl;
            Assert.Contains("https://res.cloudinary.com/djlskbceh/image/upload/", categoryImageUrl);
        }

        [Fact]
        public async Task CategoryImagesController_Update_Post_WithWrongModelState()
        {
            int categoryId = 1;

            this.controller.ModelState.AddModelError("test", "test");
            var result = await this.controller.Update(new UpdateCategoryImageBindingModel
            {
                CategoryId = categoryId,
            });

            var viewResult = Assert.IsAssignableFrom<ViewResult>(result);
            var model = Assert.IsType<UpdateCategoryImageBindingModel>(viewResult.Model);

            Assert.Equal(categoryId, model.CategoryId);
            Assert.Null(model.ImageUrl);
        }
    }
}
