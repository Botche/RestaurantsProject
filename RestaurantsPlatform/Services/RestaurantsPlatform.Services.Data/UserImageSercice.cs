namespace RestaurantsPlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;

    using static RestaurantsPlatform.Common.GlobalConstants;

    public class UserImageSercice : IUserImageSercice
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly ICloudinaryImageService imageService;

        public UserImageSercice(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            ICloudinaryImageService imageService)
        {
            this.usersRepository = usersRepository;
            this.imageService = imageService;
        }

        public async Task<string> AddDefaultImageToUserAsync(ApplicationUser user)
        {
            return await this.AddImageToUserAsync(user.UserName, DefaultProfilePicture);
        }

        public async Task<string> AddImageToUserAsync(string username, string imageUrl)
        {
            var user = this.usersRepository.All()
                .FirstOrDefault(user => user.UserName == username);

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
            var user = this.usersRepository.All()
                .FirstOrDefault(user => user.UserName == username);

            await this.imageService.DeleteImageAsync(user.PublicId);
            return await this.AddDefaultImageToUserAsync(user);
        }
    }
}
