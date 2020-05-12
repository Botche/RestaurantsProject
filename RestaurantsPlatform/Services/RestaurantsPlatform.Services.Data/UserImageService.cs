namespace RestaurantsPlatform.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;

    using static RestaurantsPlatform.Common.GlobalConstants;

    public class UserImageService : IUserImageSercice
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly ICloudinaryImageService imageService;

        public UserImageService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            ICloudinaryImageService imageService)
        {
            this.usersRepository = usersRepository;
            this.imageService = imageService;
        }

        public async Task<string> AddDefaultImageToUserAsync(ApplicationUser user)
        {
            var image = await this.imageService.UploadUserImageToCloudinaryAsync(DefaultProfilePicture);

            user.ImageUrl = image.ImageUrl;
            user.PublicId = image.PublicId;

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            return image.ImageUrl;
        }

        public async Task<string> AddImageToUserAsync(string username, string imageUrl)
        {
            var user = await this.usersRepository.All()
                .FirstOrDefaultAsync(user => user.UserName == username);

            await this.imageService.DeleteImageAsync(user.PublicId);
            var image = await this.imageService.UploadUserImageToCloudinaryAsync(imageUrl);

            user.ImageUrl = image.ImageUrl;
            user.PublicId = image.PublicId;

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            return image.ImageUrl;
        }

        public async Task<string> DeleteImageFromUserAsync(string username)
        {
            var user = await this.usersRepository.All()
                .FirstOrDefaultAsync(user => user.UserName == username);

            await this.imageService.DeleteImageAsync(user.PublicId);
            return await this.AddDefaultImageToUserAsync(user);
        }
    }
}
