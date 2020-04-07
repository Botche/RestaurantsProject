namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.AladinFoodsSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.FurnaSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.IndianKohinoorSeedInfo;
    using static RestaurantsPlatform.Data.Common.Seeding.RestaurantImages.MomaSeeding;

    public class RestaurantImagesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.RestaurantImages.Any())
            {
                return;
            }

            // Aladin
            int aladinFoodsId = dbContext.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantName == "Aladin Foods").Id;
            int aladinFoods2Id = dbContext.Restaurants
                    .Where(restaurant => restaurant.RestaurantName == "Aladin Foods")
                    .ToList()[1]
                    .Id;

            // Furna
            int furnaId = dbContext.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantName == "Furna").Id;

            // Indian restaurant kohinoor
            int indianRestaurantKohinoorId = dbContext.Restaurants
                .FirstOrDefault(restaurant => restaurant.RestaurantName == "Indian Restaurant Kohinoor").Id;

            // Indian restaurant kohinoor
            int momaId = dbContext.Restaurants
                .FirstOrDefault(restaurant => restaurant.RestaurantName == "Moma Bulgarian Food and Wine").Id;

            List<(string ImageUrl, string PublicId, int RestaurantId)> categoryImages
                = new List<(string ImageUrl, string PublicId, int RestaurantId)>
            {
                (AladinFoodsLogo, AladinFoodsLogoId, aladinFoodsId),
                (AladinFoodsFries, AladinFoodsFriesId, aladinFoodsId),
                (AladinFoodsDouner, AladinFoodsDounerId, aladinFoodsId),

                (AladinFoodsLogo2, AladinFoodsLogo2Id, aladinFoods2Id),
                (AladinFoodsFriesWithRice, AladinFoodsFriesWithRiceId, aladinFoods2Id),
                (AladinFoodsPizza, AladinFoodsPizzaId, aladinFoods2Id),

                (FurnaFirstPic, FurnaFirstPicId, furnaId),
                (FurnaSecondPic, FurnaSecondPicId, furnaId),
                (FurnaThridPic, FurnaThridPicId, furnaId),
                (FurnaFourthPic, FurnaFourthPicId, furnaId),
                (FurnaFifthPic, FurnaFifthPicId, furnaId),

                (IndianFirstPic, IndianFirstPicId, indianRestaurantKohinoorId),
                (IndianSecondPic, IndianSecondPicId, indianRestaurantKohinoorId),
                (IndianThridPic, IndianThridPicId, indianRestaurantKohinoorId),

                (MomaFirstPic, MomaFirstPicId, momaId),
                (MomaSecondPic, MomaSecondPicId, momaId),
                (MomaThridPic, MomaThridPicId, momaId),
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
    }
}
