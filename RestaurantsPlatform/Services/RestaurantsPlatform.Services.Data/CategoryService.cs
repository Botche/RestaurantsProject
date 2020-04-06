namespace RestaurantsPlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
            string cloudinaryImageUrl = this.imageService.UploadImageToCloudinary(imageUrl);
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
    }
}
