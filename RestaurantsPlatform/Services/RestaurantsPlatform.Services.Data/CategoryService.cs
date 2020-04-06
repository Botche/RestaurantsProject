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
        private readonly IImageService imageService;

        public CategoryService(
            IDeletableEntityRepository<Category> categoriesRepository,
            IImageService imageService)
        {
            this.categoriesRepository = categoriesRepository;
            this.imageService = imageService;
        }

        public async Task<int> CreateCategory(string description, string imageUrl, string name, string title)
        {
            string cloudinaryImageUrl = await this.imageService.UploadImageToCloudinaryAsync(imageUrl);
            var category = new Category
            {
                Description = description,
                Name = name,
                Title = title,
                ImageUrl = cloudinaryImageUrl,
            };

            await this.categoriesRepository.AddAsync(category);
            await this.categoriesRepository.SaveChangesAsync();

            return category.Id;
        }

        public async Task<int> DeleteCategory(int id)
        {
            var categoryToDelete = this.categoriesRepository.All()
                .Where(category => category.Id == id)
                .FirstOrDefault();

            await this.imageService.DeleteImageAsync(categoryToDelete.ImageUrl);

            this.categoriesRepository.Delete(categoryToDelete);
            await this.categoriesRepository.SaveChangesAsync();

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
            return this.categoriesRepository
                .All()
                .Where(category => category.Id == id)
                .To<T>()
                .FirstOrDefault();
        }

        public T GetByIdAndName<T>(int id, string name)
        {
            string nameWithoutDashes = name.Replace('-', ' ');

            return this.categoriesRepository
                .All()
                .Where(category => category.Id == id
                    && category.Name.ToLower() == nameWithoutDashes.ToLower())
                .To<T>()
                .FirstOrDefault();
        }

        public async Task<int> UpdateCategory(int id, string description, string name, string title)
        {
            var oldEntity = this.categoriesRepository.All()
                .Where(category => category.Id == id)
                .FirstOrDefault();

            oldEntity.Description = description;
            oldEntity.Name = name;
            oldEntity.Title = title;

            this.categoriesRepository.Update(oldEntity);
            await this.categoriesRepository.SaveChangesAsync();

            return id;
        }
    }
}
