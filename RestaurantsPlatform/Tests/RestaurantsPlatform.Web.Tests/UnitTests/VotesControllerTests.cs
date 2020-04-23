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
    using RestaurantsPlatform.Web.ViewModels.Votes;
    using Xunit;

    public class VotesControllerTests
    {
        private readonly ApplicationDbContext dbContext;

        private readonly IDeletableEntityRepository<Restaurant> restaurantRepository;
        private readonly IRepository<Vote> votesRepository;
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        private readonly ICommentService commentsService;
        private readonly IVoteService votesService;

        private readonly VotesController controller;

        public VotesControllerTests()
        {
            this.dbContext = MockDbContext.GetContext();

            this.restaurantRepository = new EfDeletableEntityRepository<Restaurant>(this.dbContext);
            this.votesRepository = new EfRepository<Vote>(this.dbContext);
            this.commentsRepository = new EfDeletableEntityRepository<Comment>(this.dbContext);

            this.commentsService = new CommentService(this.commentsRepository);
            this.votesService = new VoteService(this.votesRepository);

            this.controller = new VotesController(this.votesService);
        }

        [Fact]
        public async Task VotesController_Post_True()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.commentsService.AddCommentToRestaurantAsync(1, "Comment", "1");

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

            var result = await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = true,
            });

            var viewResult = Assert.IsType<VoteResponseModel>(result.Value);

            Assert.Equal(1, viewResult.VotesCount);

            var votesCount = this.votesRepository.All().Count();

            Assert.Equal(1, votesCount);
        }

        [Fact]
        public async Task VotesController_Post_True_AfterAlreadyTrue()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.commentsService.AddCommentToRestaurantAsync(1, "Comment", "1");

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

            await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = true,
            });
            var result = await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = true,
            });

            var viewResult = Assert.IsType<VoteResponseModel>(result.Value);

            Assert.Equal(0, viewResult.VotesCount);

            var votesCount = this.votesRepository.All().Count();

            Assert.Equal(1, votesCount);
        }

        [Fact]
        public async Task VotesController_Post_True_AfterAlreadyFalse()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.commentsService.AddCommentToRestaurantAsync(1, "Comment", "1");

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

            await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = false,
            });
            var result = await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = true,
            });

            var viewResult = Assert.IsType<VoteResponseModel>(result.Value);

            Assert.Equal(1, viewResult.VotesCount);

            var votesCount = this.votesRepository.All().Count();

            Assert.Equal(1, votesCount);
        }

        [Fact]
        public async Task VotesController_Post_False()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.commentsService.AddCommentToRestaurantAsync(1, "Comment", "1");

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

            var result = await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = false,
            });

            var viewResult = Assert.IsType<VoteResponseModel>(result.Value);

            Assert.Equal(-1, viewResult.VotesCount);

            var votesCount = this.votesRepository.All().Count();

            Assert.Equal(1, votesCount);
        }

        [Fact]
        public async Task VotesController_Post_False_AfterAlreadyTrue()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.commentsService.AddCommentToRestaurantAsync(1, "Comment", "1");

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

            await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = true,
            });
            var result = await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = false,
            });

            var viewResult = Assert.IsType<VoteResponseModel>(result.Value);

            Assert.Equal(-1, viewResult.VotesCount);

            var votesCount = this.votesRepository.All().Count();

            Assert.Equal(1, votesCount);
        }

        [Fact]
        public async Task VotesController_Post_False_AfterAlreadyFalse()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.commentsService.AddCommentToRestaurantAsync(1, "Comment", "1");

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

            await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = false,
            });
            var result = await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = false,
            });

            var viewResult = Assert.IsType<VoteResponseModel>(result.Value);

            Assert.Equal(0, viewResult.VotesCount);

            var votesCount = this.votesRepository.All().Count();

            Assert.Equal(1, votesCount);
        }

        [Fact]
        public async Task VotesController_Post_True_AfterAlreadyTrueFromOtherUser()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.commentsService.AddCommentToRestaurantAsync(1, "Comment", "1");

            await this.votesRepository.AddAsync(new Vote
            {
                UserId = "2",
                CommentId = 1,
                Type = VoteType.UpVote,
            });
            await this.votesRepository.SaveChangesAsync();

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

            var result = await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = true,
            });

            var viewResult = Assert.IsType<VoteResponseModel>(result.Value);

            Assert.Equal(2, viewResult.VotesCount);

            var votesCount = this.votesRepository.All().Count();

            Assert.Equal(2, votesCount);
        }

        [Fact]
        public async Task VotesController_Post_True_AfterAlreadyFalseFromOtherUser()
        {
            AutoMapperConfig.RegisterMappings(typeof(FavouriteRestaurantsInputModel).Assembly);

            await this.restaurantRepository.AddAsync(new Restaurant
            {
                Id = 1,
                RestaurantName = "Restaurant",
                UserId = "1",
            });
            await this.restaurantRepository.SaveChangesAsync();

            await this.commentsService.AddCommentToRestaurantAsync(1, "Comment", "1");

            await this.votesRepository.AddAsync(new Vote
            {
                UserId = "2",
                CommentId = 1,
                Type = VoteType.DownVote,
            });
            await this.votesRepository.SaveChangesAsync();

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

            var result = await this.controller.Post(new VoteInputModel
            {
                CommentId = "1",
                IsUpVote = true,
            });

            var viewResult = Assert.IsType<VoteResponseModel>(result.Value);

            Assert.Equal(0, viewResult.VotesCount);

            var votesCount = this.votesRepository.All().Count();

            Assert.Equal(2, votesCount);
        }
    }
}
