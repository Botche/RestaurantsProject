namespace RestaurantsPlatform.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    public class CategoryService : ICategoryService
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly ICategoryImageService imageService;
        private readonly IRestaurantService restaurantService;

        public CategoryService(
            IDeletableEntityRepository<Category> categoriesRepository,
            ICategoryImageService imageService,
            IRestaurantService restaurantService)
        {
            this.categoriesRepository = categoriesRepository;
            this.imageService = imageService;
            this.restaurantService = restaurantService;
        }

        public async Task<int> CreateCategoryAsync(string description, string imageUrl, string name, string title)
        {
            int imageId = await this.imageService.AddImageToCategoryAsync(imageUrl, name);
            var category = new Category
            {
                Description = description,
                Name = name,
                Title = title,
                ImageId = imageId,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public async Task<int> DeleteCategoryAsync(int id)
        {
            var categoryToDelete = await this.GetCategoryById(id)
                .FirstOrDefaultAsync();

            await this.restaurantService.DeleteAllRestaurantsAppenedToCategoryAsync(categoryToDelete.Id);

            this.categoriesRepository.Delete(categoryToDelete);
            await this.categoriesRepository.SaveChangesAsync();

            await this.imageService.DeleteImageAsync(categoryToDelete.ImageId);

            return categoryToDelete.Id;
        }

        public IEnumerable<T> GetAllCategories<T>()
        {
            IQueryable<Category> query = this.categoriesRepository.All()
                .OrderBy(category => category.Name);

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllCategoriesWithPage<T>(int? take = null, int skip = 0)
        {
            var categories = this.categoriesRepository.All()
                  .OrderBy(restaurant => restaurant.Name)
                  .Skip(skip);

            if (take.HasValue)
            {
                categories = categories.Take(take.Value);
            }

            return categories.To<T>().ToList();
        }

        public Task<T> GetByIdAsync<T>(int id)
        {
            return this.GetCategoryById(id)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public Task<T> GetByIdAndNameAsync<T>(int id, string name)
        {
            string nameWithoutDashes = name.Replace('-', ' ');

            return this.GetCategoryById(id)
                .Where(category => category.Name.ToLower() == nameWithoutDashes.ToLower())
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public int GetCountOfAllCategories()
        {
            return this.categoriesRepository.All().Count();
        }

        public async Task<int> UpdateCategoryAsync(int id, string description, string name, string title)
        {
            var oldEntity = await this.GetCategoryById(id)
                .FirstOrDefaultAsync();

            oldEntity.Description = description;
            oldEntity.Name = name;
            oldEntity.Title = title;

            this.categoriesRepository.Update(oldEntity);
            await this.categoriesRepository.SaveChangesAsync();

            return id;
        }

        public async Task<int> UpdateCategoryImageAsync(int categoryId, string imageUrl)
        {
            var imageInfo = await this.GetByIdAsync<PublicIdCategoryImageBindinModel>(categoryId);

            await this.imageService.DeleteImageAsync(imageInfo.ImageId);
            var imageId = await this.imageService.AddImageToCategoryAsync(imageUrl, imageInfo.Name);

            var category = this.GetCategoryById(categoryId).FirstOrDefault();
            category.ImageId = imageId;

            this.categoriesRepository.Update(category);
            await this.categoriesRepository.SaveChangesAsync();

            return imageId;
        }

        private IQueryable<Category> GetCategoryById(int id)
        {
            return this.categoriesRepository
                .All()
                .Where(resturant => resturant.Id == id);
        }
    }
}
