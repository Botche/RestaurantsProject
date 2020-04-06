namespace RestaurantsPlatform.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Services.Mapping;

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
            var categoryToDelete = this.GetCategoryById(id)
                .FirstOrDefault();

            await this.restaurantService.DeleteAllRestaurantsAppenedToCategoryAsync(categoryToDelete.Id);

            this.categoriesRepository.Delete(categoryToDelete);
            await this.categoriesRepository.SaveChangesAsync();

            await this.imageService.DeleteImageAsync(categoryToDelete.ImageId);

            return categoryToDelete.Id;
        }

        public IEnumerable<T> GetAllCategories<T>(int? count = null)
        {
            IQueryable<Category> query = this.categoriesRepository.All()
                .OrderBy(category => category.Name);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(int id)
        {
            return this.GetCategoryById(id)
                .To<T>()
                .FirstOrDefault();
        }

        public T GetByIdAndName<T>(int id, string name)
        {
            string nameWithoutDashes = name.Replace('-', ' ');

            return this.GetCategoryById(id)
                .Where(category => category.Name.ToLower() == nameWithoutDashes.ToLower())
                .To<T>()
                .FirstOrDefault();
        }

        public async Task<int> UpdateCategoryAsync(int id, string description, string name, string title)
        {
            var oldEntity = this.GetCategoryById(id)
                .FirstOrDefault();

            oldEntity.Description = description;
            oldEntity.Name = name;
            oldEntity.Title = title;

            this.categoriesRepository.Update(oldEntity);
            await this.categoriesRepository.SaveChangesAsync();

            return id;
        }

        private IQueryable<Category> GetCategoryById(int id)
        {
            return this.categoriesRepository
                .All()
                .Where(resturant => resturant.Id == id);
        }
    }
}
