// <copyright file="RestaurantImagesSeeder.cs" company="RestaurantsPlatform">
// Copyright (c) RestaurantsPlatform. All Rights Reserved.
// </copyright>

namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Common.Seeding.RestaurantImages;
    using RestaurantsPlatform.Data.Models.Restaurants;
    using RestaurantsPlatform.Services.Data.Interfaces;
    using RestaurantsPlatform.Web.ViewModels.CategoryImages;

    using static RestaurantsPlatform.Common.StringExtensions;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.AladinFoodsSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.FurnaSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.IndianKohinoorSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.MomaSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.SkaptoSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.WokToWalkSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.Restaurants.SeedInfo;

    /// <summary>
    /// Category seeder.
    /// </summary>
    public class RestaurantImagesSeeder : ISeeder
    {
        private readonly ICloudinaryImageService imageService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestaurantImagesSeeder"/> class.
        /// </summary>
        /// <param name="imageService">Image service to upload images to cloud.</param>
        public RestaurantImagesSeeder(ICloudinaryImageService imageService)
        {
            this.imageService = imageService;
        }

        /// <summary>
        /// Seeding method.
        /// </summary>
        /// <param name="dbContext">Database.</param>
        /// <param name="serviceProvider">Service provider.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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
            var aladinFoodImageLogo =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsLogo, aladinFoods.RestaurantName.ToSlug());
            var aladinFoodImageFries =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsFries, aladinFoods.RestaurantName.ToSlug());
            var aladinFoodImageDouner =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsDouner, aladinFoods.RestaurantName.ToSlug());

            var aladinFoods2 = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .Where(restaurant => restaurant.RestaurantName == Aladin2Name)
                .ToList()[1];

            var aladinFoodImageLogo2 =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsLogo2, aladinFoods2.RestaurantName.ToSlug());
            var aladinFoodImageFriesWithRice =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsFriesWithRice, aladinFoods2.RestaurantName.ToSlug());
            var aladinFoodImagePizza =
                await this.UploadImagesToCloudinaryAsync(AladinFoodsPizza, aladinFoods2.RestaurantName.ToSlug());

            // Furna
            var furna = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == FurnaName);

            var furnaFirst =
                await this.UploadImagesToCloudinaryAsync(FurnaFirstPic, furna.RestaurantName.ToSlug());
            var furnaSecond =
                await this.UploadImagesToCloudinaryAsync(FurnaSecondPic, furna.RestaurantName.ToSlug());
            var furnaThird =
                await this.UploadImagesToCloudinaryAsync(FurnaThirdPic, furna.RestaurantName.ToSlug());
            var furnaFourth =
                await this.UploadImagesToCloudinaryAsync(FurnaFourthPic, furna.RestaurantName.ToSlug());
            var furnaFifth =
                await this.UploadImagesToCloudinaryAsync(FurnaFifthPic, furna.RestaurantName.ToSlug());

            // Indian restaurant kohinoor
            var indian = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == IndianName);

            var indianFirst =
                await this.UploadImagesToCloudinaryAsync(IndianFirstPic, indian.RestaurantName.ToSlug());
            var indiandSecond =
                await this.UploadImagesToCloudinaryAsync(IndianSecondPic, indian.RestaurantName.ToSlug());
            var indianThird =
                await this.UploadImagesToCloudinaryAsync(IndianThirdPic, indian.RestaurantName.ToSlug());

            // Moma
            var moma = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == MomaName);

            var momaFirst = await this.UploadImagesToCloudinaryAsync(MomaFirstPic, moma.RestaurantName.ToSlug());
            var momaSecond = await this.UploadImagesToCloudinaryAsync(MomaSecondPic, moma.RestaurantName.ToSlug());
            var momaThird = await this.UploadImagesToCloudinaryAsync(MomaThirdPic, moma.RestaurantName.ToSlug());

            // Skapto
            var skapto = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == SkaptoName);

            var skaptoFirst =
                await this.UploadImagesToCloudinaryAsync(SkaptoFirstPic, skapto.RestaurantName.ToSlug());
            var skaptoSecond =
                await this.UploadImagesToCloudinaryAsync(SkaptoSecondPic, skapto.RestaurantName.ToSlug());
            var skaptoThird =
                await this.UploadImagesToCloudinaryAsync(SkaptoThirdPic, skapto.RestaurantName.ToSlug());
            var skaptoFourth =
                await this.UploadImagesToCloudinaryAsync(SkaptoFourthPic, skapto.RestaurantName.ToSlug());

            // Wok
            var wok = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == WokName);

            var wokFirst = await this.UploadImagesToCloudinaryAsync(WokFirstPic, wok.RestaurantName.ToSlug());
            var wokSecond = await this.UploadImagesToCloudinaryAsync(WokSecondPic, wok.RestaurantName.ToSlug());
            var wokThird = await this.UploadImagesToCloudinaryAsync(WokThirdPic, wok.RestaurantName.ToSlug());

            // Gurkha
            var gurkha = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == GurkhaName);

            var gurkhaFirst = await this.UploadImagesToCloudinaryAsync(GurkhaSeedInfo.FirstImage, gurkha.RestaurantName.ToSlug());
            var gurkhaSecond = await this.UploadImagesToCloudinaryAsync(GurkhaSeedInfo.SecondImage, gurkha.RestaurantName.ToSlug());

            // Memento
            var memento = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == MementoName);

            var mementoFirst = await this.UploadImagesToCloudinaryAsync(MementoSeedInfo.FirstImage, memento.RestaurantName.ToSlug());
            var mementoSecond = await this.UploadImagesToCloudinaryAsync(MementoSeedInfo.SecondImage, memento.RestaurantName.ToSlug());
            var mementoThird = await this.UploadImagesToCloudinaryAsync(MementoSeedInfo.ThirdImage, memento.RestaurantName.ToSlug());
            var mementoFourth = await this.UploadImagesToCloudinaryAsync(MementoSeedInfo.FourthImage, memento.RestaurantName.ToSlug());
            var mementoFifth = await this.UploadImagesToCloudinaryAsync(MementoSeedInfo.FifthImage, memento.RestaurantName.ToSlug());
            var mementoSixth = await this.UploadImagesToCloudinaryAsync(MementoSeedInfo.SixthImage, memento.RestaurantName.ToSlug());
            var mementoSeventh = await this.UploadImagesToCloudinaryAsync(MementoSeedInfo.SeventhImage, memento.RestaurantName.ToSlug());

            // Rainbow
            var rainbow = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == RainbowName);

            var rainbowFirst = await this.UploadImagesToCloudinaryAsync(RainbowSeedInfo.FirstImage, rainbow.RestaurantName.ToSlug());
            var rainbowSecond = await this.UploadImagesToCloudinaryAsync(RainbowSeedInfo.SecondImage, rainbow.RestaurantName.ToSlug());
            var rainbowThird = await this.UploadImagesToCloudinaryAsync(RainbowSeedInfo.ThirdImage, rainbow.RestaurantName.ToSlug());
            var rainbowFourth = await this.UploadImagesToCloudinaryAsync(RainbowSeedInfo.FourthImage, rainbow.RestaurantName.ToSlug());
            var rainbowFifth = await this.UploadImagesToCloudinaryAsync(RainbowSeedInfo.FifthImage, rainbow.RestaurantName.ToSlug());
            var rainbowSixth = await this.UploadImagesToCloudinaryAsync(RainbowSeedInfo.SixthImage, rainbow.RestaurantName.ToSlug());

            // Rosiche
            var rosiche = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == RosicheName);

            var rosicheFirst = await this.UploadImagesToCloudinaryAsync(RosicheSeedInfo.FirstImage, rosiche.RestaurantName.ToSlug());
            var rosicheSecond = await this.UploadImagesToCloudinaryAsync(RosicheSeedInfo.SecondImage, rosiche.RestaurantName.ToSlug());
            var rosicheThird = await this.UploadImagesToCloudinaryAsync(RosicheSeedInfo.ThirdImage, rosiche.RestaurantName.ToSlug());
            var rosicheFourth = await this.UploadImagesToCloudinaryAsync(RosicheSeedInfo.FourthImage, rosiche.RestaurantName.ToSlug());

            // Tenebris
            var tenebris = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == TenebrisName);

            var tenebrisFirst = await this.UploadImagesToCloudinaryAsync(TenebrisSeedInfo.FirstImage, tenebris.RestaurantName.ToSlug());
            var tenebrisSecond = await this.UploadImagesToCloudinaryAsync(TenebrisSeedInfo.SecondImage, tenebris.RestaurantName.ToSlug());
            var tenebrisThird = await this.UploadImagesToCloudinaryAsync(TenebrisSeedInfo.ThirdImage, tenebris.RestaurantName.ToSlug());
            var tenebrisFourth = await this.UploadImagesToCloudinaryAsync(TenebrisSeedInfo.FourthImage, tenebris.RestaurantName.ToSlug());
            var tenebrisFifth = await this.UploadImagesToCloudinaryAsync(TenebrisSeedInfo.FifthImage, tenebris.RestaurantName.ToSlug());

            // Cosmos
            var cosmos = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == CosmosName);

            var cosmosFirst = await this.UploadImagesToCloudinaryAsync(CosmosSeedInfo.FirstImage, cosmos.RestaurantName.ToSlug());
            var cosmosSecond = await this.UploadImagesToCloudinaryAsync(CosmosSeedInfo.SecondImage, cosmos.RestaurantName.ToSlug());
            var cosmosThird = await this.UploadImagesToCloudinaryAsync(CosmosSeedInfo.ThirdImage, cosmos.RestaurantName.ToSlug());
            var cosmosFourth = await this.UploadImagesToCloudinaryAsync(CosmosSeedInfo.FourthImage, cosmos.RestaurantName.ToSlug());

            // Bottega
            var bottega = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == BottegaName);

            var bottegaFirst = await this.UploadImagesToCloudinaryAsync(BottegaSeedInfo.FirstImage, bottega.RestaurantName.ToSlug());
            var bottegaSecond = await this.UploadImagesToCloudinaryAsync(BottegaSeedInfo.SecondImage, bottega.RestaurantName.ToSlug());

            // Sputnik
            var sputnik = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == SputnikName);

            var sputnikFirst = await this.UploadImagesToCloudinaryAsync(SputnikSeedInfo.FirstImage, sputnik.RestaurantName.ToSlug());
            var sputnikSecond = await this.UploadImagesToCloudinaryAsync(SputnikSeedInfo.SecondImage, sputnik.RestaurantName.ToSlug());
            var sputnikThird = await this.UploadImagesToCloudinaryAsync(SputnikSeedInfo.ThirdImage, sputnik.RestaurantName.ToSlug());

            // Cocktail
            var cocktail = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == CocktailName);

            var cocktailFirst = await this.UploadImagesToCloudinaryAsync(CocktailSeedInfo.FirstImage, cocktail.RestaurantName.ToSlug());
            var cocktailSecond = await this.UploadImagesToCloudinaryAsync(CocktailSeedInfo.SecondImage, cocktail.RestaurantName.ToSlug());
            var cocktailThird = await this.UploadImagesToCloudinaryAsync(CocktailSeedInfo.ThirdImage, cocktail.RestaurantName.ToSlug());

            // Gastro
            var gastro = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == GastroName);

            var gastroFirst = await this.UploadImagesToCloudinaryAsync(GastroSeedInfo.FirstImage, gastro.RestaurantName.ToSlug());
            var gastroSecond = await this.UploadImagesToCloudinaryAsync(GastroSeedInfo.SecondImage, gastro.RestaurantName.ToSlug());

            // Road
            var road = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == RoadName);

            var roadFirst = await this.UploadImagesToCloudinaryAsync(RoadSeedInfo.FirstImage, road.RestaurantName.ToSlug());
            var roadSecond = await this.UploadImagesToCloudinaryAsync(RoadSeedInfo.SecondImage, road.RestaurantName.ToSlug());
            var roadThird = await this.UploadImagesToCloudinaryAsync(RoadSeedInfo.ThirdImage, road.RestaurantName.ToSlug());

            // Oscar
            var oscar = dbContext.Restaurants
                .Select(restaurant => new
                {
                    restaurant.Id,
                    restaurant.RestaurantName,
                })
                .FirstOrDefault(restaurant => restaurant.RestaurantName == OscarName);

            var oscarFirst = await this.UploadImagesToCloudinaryAsync(OscarSeedInfo.FirstImage, oscar.RestaurantName.ToSlug());
            var oscarSecond = await this.UploadImagesToCloudinaryAsync(OscarSeedInfo.SecondImage, oscar.RestaurantName.ToSlug());
            var oscarThird = await this.UploadImagesToCloudinaryAsync(OscarSeedInfo.ThirdImage, oscar.RestaurantName.ToSlug());
            var oscarFourth = await this.UploadImagesToCloudinaryAsync(OscarSeedInfo.FourhtImage, oscar.RestaurantName.ToSlug());

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

                (gurkhaFirst.ImageUrl, gurkhaFirst.PublicId, gurkha.Id),
                (gurkhaSecond.ImageUrl, gurkhaSecond.PublicId, gurkha.Id),

                (mementoFirst.ImageUrl, mementoFirst.PublicId, memento.Id),
                (mementoSecond.ImageUrl, mementoSecond.PublicId, memento.Id),
                (mementoThird.ImageUrl, mementoThird.PublicId, memento.Id),
                (mementoFourth.ImageUrl, mementoFourth.PublicId, memento.Id),
                (mementoFifth.ImageUrl, mementoFifth.PublicId, memento.Id),
                (mementoSixth.ImageUrl, mementoSixth.PublicId, memento.Id),
                (mementoSeventh.ImageUrl, mementoSeventh.PublicId, memento.Id),

                (rainbowFirst.ImageUrl, rainbowFirst.PublicId, rainbow.Id),
                (rainbowSecond.ImageUrl, rainbowSecond.PublicId, rainbow.Id),
                (rainbowThird.ImageUrl, rainbowThird.PublicId, rainbow.Id),
                (rainbowFourth.ImageUrl, rainbowFourth.PublicId, rainbow.Id),
                (rainbowFifth.ImageUrl, rainbowFifth.PublicId, rainbow.Id),
                (rainbowSixth.ImageUrl, rainbowSixth.PublicId, rainbow.Id),

                (rosicheFirst.ImageUrl, rosicheFirst.PublicId, rosiche.Id),
                (rosicheSecond.ImageUrl, rosicheSecond.PublicId, rosiche.Id),
                (rosicheThird.ImageUrl, rosicheThird.PublicId, rosiche.Id),
                (rosicheFourth.ImageUrl, rosicheFourth.PublicId, rosiche.Id),

                (tenebrisFirst.ImageUrl, tenebrisFirst.PublicId, tenebris.Id),
                (tenebrisSecond.ImageUrl, tenebrisSecond.PublicId, tenebris.Id),
                (tenebrisThird.ImageUrl, tenebrisThird.PublicId, tenebris.Id),
                (tenebrisFourth.ImageUrl, tenebrisFourth.PublicId, tenebris.Id),
                (tenebrisFifth.ImageUrl, tenebrisFifth.PublicId, tenebris.Id),

                (cosmosFirst.ImageUrl, cosmosFirst.PublicId, cosmos.Id),
                (cosmosSecond.ImageUrl, cosmosSecond.PublicId, cosmos.Id),
                (cosmosThird.ImageUrl, cosmosThird.PublicId, cosmos.Id),
                (cosmosFourth.ImageUrl, cosmosFourth.PublicId, cosmos.Id),

                (bottegaFirst.ImageUrl, bottegaFirst.PublicId, bottega.Id),
                (bottegaSecond.ImageUrl, bottegaSecond.PublicId, bottega.Id),

                (sputnikFirst.ImageUrl, sputnikFirst.PublicId, sputnik.Id),
                (sputnikSecond.ImageUrl, sputnikSecond.PublicId, sputnik.Id),
                (sputnikThird.ImageUrl, sputnikThird.PublicId, sputnik.Id),

                (cocktailFirst.ImageUrl, cocktailFirst.PublicId, cocktail.Id),
                (cocktailSecond.ImageUrl, cocktailSecond.PublicId, cocktail.Id),
                (cocktailThird.ImageUrl, cocktailThird.PublicId, cocktail.Id),

                (gastroFirst.ImageUrl, gastroFirst.PublicId, gastro.Id),
                (gastroSecond.ImageUrl, gastroSecond.PublicId, gastro.Id),

                (roadFirst.ImageUrl, roadFirst.PublicId, road.Id),
                (roadSecond.ImageUrl, roadSecond.PublicId, road.Id),
                (roadThird.ImageUrl, roadThird.PublicId, road.Id),

                (oscarFirst.ImageUrl, oscarFirst.PublicId, oscar.Id),
                (oscarSecond.ImageUrl, oscarSecond.PublicId, oscar.Id),
                (oscarThird.ImageUrl, oscarThird.PublicId, oscar.Id),
                (oscarFourth.ImageUrl, oscarFourth.PublicId, oscar.Id),
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
