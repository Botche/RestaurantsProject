namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.AladinFoodsSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.FurnaSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.IndianKohinoorSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.MomaSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.SkaptoSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.WokToWalkSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.Restaurants.SeedInfo;

    public class RestaurantImagesSeeder : ISeeder
    {
        private readonly ICloudinaryImageService imageService;

        public RestaurantImagesSeeder(ICloudinaryImageService imageService)
        {
            this.imageService = imageService;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.RestaurantImages.Any())
            {
                return;
            }

            // Aladin
            var aladinFoods = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == AladinName);
            var aladinFoods2 = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .Where(restaurant => restaurant.RestaurantName == Aladin2Name)
                .ToList()[1];

            // Furna
            var furna = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == FurnaName);

            // Indian restaurant kohinoor
            var indian = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == IndianName);

            // Indian restaurant kohinoor
            var moma = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == MomaName);

            // Indian restaurant kohinoor
            var skapto = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == SkaptoName);

            // Indian restaurant kohinoor
            var wok = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == WokName);

            var aladinFoodImageLogo =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsLogo, aladinFoods.RestaurantName.ToLower().Replace(" ", "-"));
            var aladinFoodImageFries =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsFries, aladinFoods.RestaurantName.ToLower().Replace(" ", "-"));
            var aladinFoodImageDouner =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsDouner, aladinFoods.RestaurantName.ToLower().Replace(" ", "-"));

            var aladinFoodImageLogo2 =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsLogo2, aladinFoods2.RestaurantName.ToLower().Replace(" ", "-"));
            var aladinFoodImageFriesWithRice =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsFriesWithRice, aladinFoods2.RestaurantName.ToLower().Replace(" ", "-"));
            var aladinFoodImagePizza =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsPizza, aladinFoods2.RestaurantName.ToLower().Replace(" ", "-"));

            var furnaFirst =
                await this.UploadImagesToCloudinaryAsync(FurnaFirstPic, furna.RestaurantName.ToLower().Replace(" ", "-"));
            var furnaSecond =
                await this.UploadImagesToCloudinaryAsync(FurnaSecondPic, furna.RestaurantName.ToLower().Replace(" ", "-"));
            var furnaThird =
                await this.UploadImagesToCloudinaryAsync(FurnaThirdPic, furna.RestaurantName.ToLower().Replace(" ", "-"));
            var furnaFourth =
                await this.UploadImagesToCloudinaryAsync(FurnaFourthPic, furna.RestaurantName.ToLower().Replace(" ", "-"));
            var furnaFifth =
                await this.UploadImagesToCloudinaryAsync(FurnaFifthPic, furna.RestaurantName.ToLower().Replace(" ", "-"));

            var indianFirst =
                await this.UploadImagesToCloudinaryAsync(IndianFirstPic, indian.RestaurantName.ToLower().Replace(" ", "-"));
            var indiandSecond =
                await this.UploadImagesToCloudinaryAsync(IndianSecondPic, indian.RestaurantName.ToLower().Replace(" ", "-"));
            var indianThird =
                await this.UploadImagesToCloudinaryAsync(IndianThirdPic, indian.RestaurantName.ToLower().Replace(" ", "-"));

            var momaFirst = await this.UploadImagesToCloudinaryAsync(MomaFirstPic, moma.RestaurantName.ToLower().Replace(" ", "-"));
            var momaSecond = await this.UploadImagesToCloudinaryAsync(MomaSecondPic, moma.RestaurantName.ToLower().Replace(" ", "-"));
            var momaThird = await this.UploadImagesToCloudinaryAsync(MomaThirdPic, moma.RestaurantName.ToLower().Replace(" ", "-"));

            var skaptoFirst =
                await this.UploadImagesToCloudinaryAsync(SkaptoFirstPic, skapto.RestaurantName.ToLower().Replace(" ", "-"));
            var skaptoSecond =
                await this.UploadImagesToCloudinaryAsync(SkaptoSecondPic, skapto.RestaurantName.ToLower().Replace(" ", "-"));
            var skaptoThird =
                await this.UploadImagesToCloudinaryAsync(SkaptoThirdPic, skapto.RestaurantName.ToLower().Replace(" ", "-"));
            var skaptoFourth =
                await this.UploadImagesToCloudinaryAsync(SkaptoFourthPic, skapto.RestaurantName.ToLower().Replace(" ", "-"));

            var wokFirst = await this.UploadImagesToCloudinaryAsync(WokFirstPic, wok.RestaurantName.ToLower().Replace(" ", "-"));
            var wokSecond = await this.UploadImagesToCloudinaryAsync(WokSecondPic, wok.RestaurantName.ToLower().Replace(" ", "-"));
            var wokThird = await this.UploadImagesToCloudinaryAsync(WokThirdPic, wok.RestaurantName.ToLower().Replace(" ", "-"));

            List<(string ImageUrl, string PublicId, int RestaurantId)> categoryImages
                = new List<(string ImageUrl, string PublicId, int RestaurantId)>
            {
                (aladinFoodImageLogo.ImageUrl, aladinFoodImageLogo.PublicId, aladinFoods.Id),
                (aladinFoodImageFries.ImageUrl, aladinFoodImageFries.PublicId, aladinFoods.Id),
                (aladinFoodImageDouner.ImageUrl, aladinFoodImageDouner.PublicId, aladinFoods.Id),

                (aladinFoodImageLogo2.ImageUrl, aladinFoodImageLogo2.PublicId, aladinFoods2.Id),
                (aladinFoodImageFriesWithRice.ImageUrl, aladinFoodImageFriesWithRice.PublicId, aladinFoods2.Id),
                (aladinFoodImagePizza.ImageUrl, aladinFoodImagePizza.PublicId, aladinFoods2.Id),

                (furnaFirst.ImageUrl, furnaFirst.PublicId, furna.Id),
                (furnaSecond.ImageUrl, furnaSecond.PublicId, furna.Id),
                (furnaThird.ImageUrl, furnaThird.PublicId, furna.Id),
                (furnaFourth.ImageUrl, furnaFourth.PublicId, furna.Id),
                (furnaFifth.ImageUrl, furnaFifth.PublicId, furna.Id),

                (indianFirst.ImageUrl, indianFirst.PublicId, indian.Id),
                (indiandSecond.ImageUrl, indiandSecond.PublicId, indian.Id),
                (indianThird.ImageUrl, indianThird.PublicId, indian.Id),

                (momaFirst.ImageUrl, momaFirst.PublicId, moma.Id),
                (momaSecond.ImageUrl, momaSecond.PublicId, moma.Id),
                (momaThird.ImageUrl, momaThird.PublicId, moma.Id),

                (skaptoFirst.ImageUrl, skaptoFirst.PublicId, skapto.Id),
                (skaptoSecond.ImageUrl, skaptoSecond.PublicId, skapto.Id),
                (skaptoThird.ImageUrl, skaptoThird.PublicId, skapto.Id),
                (skaptoFourth.ImageUrl, skaptoFourth.PublicId, skapto.Id),

                (wokFirst.ImageUrl, wokFirst.PublicId, wok.Id),
                (wokSecond.ImageUrl, wokSecond.PublicId, wok.Id),
                (wokThird.ImageUrl, wokThird.PublicId, wok.Id),
            };

            foreach (var image in categoryImages)
            {
                await dbContext.RestaurantImages.AddAsync(new RestaurantImage
                {
                    ImageUrl = image.ImageUrl,
                    PublicId = image.PublicId,
                    RestaurantId = image.RestaurantId,
                });
            }
        }

        private async Task<ImageBindingModel> UploadImagesToCloudinaryAsync(string imageUrl, string restaurantName)
        {
            var image = await this.imageService.UploadRestaurantImageToCloudinaryAsync(imageUrl, restaurantName);

            return image;
        }
    }
}
