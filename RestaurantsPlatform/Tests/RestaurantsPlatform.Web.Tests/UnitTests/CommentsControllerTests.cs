namespace RestaurantsPlatform.Web.Tests.UnitTests
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Moq;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Data.Repositories;
    using RestaurantsPlatform.Services.Data;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.Areas.Restaurants.Controllers;
    using RestaurantsPlatform.Web.Tests.Mocks;
    using RestaurantsPlatform.Web.ViewModels;
    using RestaurantsPlatform.Web.ViewModels.Comments;
    using Xunit;

    using static RestaurantsPlatform.Web.Infrastructure.ErrorConstants;

    public class CommentsControllerTests
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IDeletableEntityRepository<Restaurant> restaurantRepository;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        private readonly ICommentService commentsService;
        private readonly CommentsController controller;

        public CommentsControllerTests()
        {
            this.dbContext = MockDbContext.GetContext();

            this.restaurantRepository = new EfDeletableEntityRepository<Restaurant>(this.dbContext);
            this.commentsRepository = new EfDeletableEntityRepository<Comment>(this.dbContext);

            this.commentsService = new CommentService(this.commentsRepository);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());

            this.controller = new CommentsController(this.commentsService)
            {
                TempData = tempData,
            };
        }

        [Fact]
        public void CommentsController_Constructor()
        {
            Assert.IsType<CommentsController>(this.controller);
        }

        [Fact]
        public async Task CommentsController_AddCommentToRestaurant()
        {
            AutoMapperConfig.RegisterMappings(typeof(CreateCommentInputModel).Assembly);

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

            var result = await this.controller.AddCommentToRestaurant(new CreateCommentInputModel
            {
                Id = 1,
                CommentContent = "Comment",
                RestaurantName = "Restaurant",
            });

            var viewResult = Assert.IsType<RedirectToRouteResult>(result);

            Assert.Equal("restaurant", viewResult.RouteName);

            var commentsCount = this.restaurantRepository.All()
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.Comments,
                })
                .FirstOrDefault(restaurant => restaurant.Id == 1)
                .Comments
                .Count();

            Assert.Equal(1, commentsCount);
        }

        [Fact]
        public async Task CommentsController_AddCommentToRestaurant_InvalidModelState()
        {
            AutoMapperConfig.RegisterMappings(typeof(CreateCommentInputModel).Assembly);

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
            var result = await this.controller.AddCommentToRestaurant(new CreateCommentInputModel
            {
                Id = 1,
                RestaurantName = "Restaurant",
            });

            var viewResult = Assert.IsType<RedirectToRouteResult>(result);

            Assert.Equal("restaurant", viewResult.RouteName);

            var commentsCount = this.restaurantRepository.All()
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.Comments,
                })
                .FirstOrDefault(restaurant => restaurant.Id == 1)
                .Comments
                .Count();

            Assert.Equal(0, commentsCount);
        }

        [Fact]
        public async Task CommentsController_Delete()
        {
            AutoMapperConfig.RegisterMappings(typeof(CreateCommentInputModel).Assembly);

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

            await this.controller.AddCommentToRestaurant(new CreateCommentInputModel
            {
                Id = 1,
                CommentContent = "Comment",
                RestaurantName = "Restaurant",
            });

            var result = await this.controller.Delete(new DeleteCommentInputModel
            {
                Id = 1,
                CommentId = 1,
                Name = "Restaurant",
            });

            var viewResult = Assert.IsType<RedirectToRouteResult>(result);

            Assert.Equal("restaurant", viewResult.RouteName);

            var commentsCount = this.restaurantRepository.All()
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.Comments,
                })
                .FirstOrDefault(restaurant => restaurant.Id == 1)
                .Comments
                .Count();

            Assert.Equal(0, commentsCount);
        }

        [Fact]
        public async Task CommentsController_Delete_WrongId()
        {
            AutoMapperConfig.RegisterMappings(typeof(CreateCommentInputModel).Assembly);

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

            await this.controller.AddCommentToRestaurant(new CreateCommentInputModel
            {
                Id = 1,
                CommentContent = "Comment",
                RestaurantName = "Restaurant",
            });

            var result = await this.controller.Delete(new DeleteCommentInputModel
            {
                Id = 1,
                CommentId = 2,
                Name = "Restaurant",
            });

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.Model);

            Assert.Equal(404, model.StatusCode);
            Assert.Equal(CommentNotFound, model.Message);

            var commentsCount = this.restaurantRepository.All()
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.Comments,
                })
                .FirstOrDefault(restaurant => restaurant.Id == 1)
                .Comments
                .Count();

            Assert.Equal(1, commentsCount);
        }

        [Fact]
        public async Task CommentsController_Update()
        {
            AutoMapperConfig.RegisterMappings(typeof(CreateCommentInputModel).Assembly);

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

            await this.controller.AddCommentToRestaurant(new CreateCommentInputModel
            {
                Id = 1,
                CommentContent = "Comment",
                RestaurantName = "Restaurant",
            });

            var result = await this.controller.Update(new UpdateCommentInputView
            {
                CommentId = 1,
                Content = "New Comment",
            });

            var viewResult = Assert.IsType<JsonResult>(result);

            var comment = this.restaurantRepository.All()
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.Comments,
                })
                .FirstOrDefault(restaurant => restaurant.Id == 1)
                .Comments
                .FirstOrDefault();

            Assert.Equal(1, comment.Id);
            Assert.Equal("New Comment", comment.Content);
        }

        [Fact]
        public async Task CommentsController_Update_InvalidModelState()
        {
            AutoMapperConfig.RegisterMappings(typeof(CreateCommentInputModel).Assembly);

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

            await this.controller.AddCommentToRestaurant(new CreateCommentInputModel
            {
                Id = 1,
                CommentContent = "Comment",
                RestaurantName = "Restaurant",
            });

            this.controller.ModelState.AddModelError("error", "error");
            var result = await this.controller.Update(new UpdateCommentInputView
            {
                CommentId = 1,
                Content = "New Comment",
            });

            var viewResult = Assert.IsType<BadRequestResult>(result);

            var comment = this.restaurantRepository.All()
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.Comments,
                })
                .FirstOrDefault(restaurant => restaurant.Id == 1)
                .Comments
                .FirstOrDefault();

            Assert.Equal(1, comment.Id);
            Assert.Equal("Comment", comment.Content);
        }
    }
}
