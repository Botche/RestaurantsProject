namespace RestaurantsPlatform.Seed.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RestaurantsPlatform.Data;
    using RestaurantsPlatform.Data.Models.Restaurants;

    public class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            string ethnic = "https://res.cloudinary.com/djlskbceh/image/upload/v1585916253/restaurant/categories/Ethnic_pa9sza.jpg";
            string fastFood = "https://res.cloudinary.com/djlskbceh/image/upload/v1585915709/restaurant/categories/FastFoodRestaurant_fioa7v.jpg";
            string casualDining = "https://res.cloudinary.com/djlskbceh/image/upload/v1585916495/restaurant/categories/Casual_gq5sfq.jpg";
            string premiumCasual = "https://res.cloudinary.com/djlskbceh/image/upload/v1585916491/restaurant/categories/PremiumDining_xxfwqi.jpg";
            string familyStyle = "https://res.cloudinary.com/djlskbceh/image/upload/v1585916190/restaurant/categories/FamilyStyle_l2takm.jpg";
            string fineDining = "https://res.cloudinary.com/djlskbceh/image/upload/v1585916188/restaurant/categories/FineDining_usmlez.jpg";
            string cafeteria = "https://res.cloudinary.com/djlskbceh/image/upload/v1585916185/restaurant/categories/Cafeteria_ety9so.jpg";
            string pub = "https://res.cloudinary.com/djlskbceh/image/upload/v1585916179/restaurant/categories/Pub_m94xos.jpg";

            List <(string Name, string ImageId, string Description)> categories
                = new List<(string Name, string ImageId, string Description)>
            {
                ("Ethnic",
                    ethnic,
                    "Ethnic restaurants specialize in ethnic or national cuisines. For example, Greek restaurants specialize in Greek cuisine."),
                ("Fast food",
                    fastFood,
                    "Fast food restaurants emphasize speed of service. Operations range from small-scale street vendors with food carts to multibillion-dollar corporations like McDonald's and Burger King. Food is ordered not from the table, but from a front counter (or in some cases, using an electronic terminal). Diners typically then carry their own food from the counter to a table of their choosing, and afterward dispose of any waste from their trays. Drive-through and take-out service may also be available. Fast food restaurants are known in the restaurant industry as QSRs or quick-service restaurants."),
                ("Casual dining",
                    casualDining,
                    "A casual dining restaurant (or sit down restaurant) is a restaurant that serves moderately priced food in a casual atmosphere. Except for buffet-style restaurants, casual dining restaurants typically provide table service. Chain examples include Harvester in the United Kingdom and TGI Fridays in the United States. Casual dining comprises a market segment between fast-food establishments and fine-dining restaurants. Casual-dining restaurants often have a full bar with separate bar staff, a full beer menu and a limited wine menu. They are frequently, but not necessarily, part of a wider chain, particularly in the US. In Italy, such casual restaurants are often called \"trattoria\", and are usually independently owned and operated."),
                ("Premium casual",
                    premiumCasual,
                    "Premium casual restaurants originate from Western Canada and include chains such as Cactus Club Cafe, Earl's and JOEY. Premium casual restaurants are described as upscale fast casual. Similarly to casual dining, they typically feature a dining room section and a lounge section with multiple screens. They are typically found downtown or in shopping districts. Premium casual restaurants carry a wide range of menu options including burgers, steaks, seafood, pizza, pasta and Asian foods."),
                ("Family style",
                    familyStyle,
                    "Family style restaurants are a type of casual dining restaurants where food is often served on platters and the diners serve themselves. It can also be used to describe family-friendly diners or casual restaurants."),
                ("Fine dining",
                    fineDining,
                    "Fine dining restaurants are full-service restaurants with specific dedicated meal courses. Décor of such restaurants features higher-quality materials, with establishments having certain rules of dining which visitors are generally expected to follow, sometimes including a dress code. Fine dining establishments are sometimes called white-tablecloth restaurants, because they traditionally featured table service by servers, at tables covered by white tablecloths. The tablecloths came to symbolize the experience. The use of white tablecloths eventually became less fashionable, but the service and upscale ambiance remained."),
                ("Cafeteria",
                    cafeteria,
                    "A cafeteria is a restaurant serving ready-cooked food arranged behind a food-serving counter. There is no table service. Typically, a patron takes a tray and pushes it along a track in front of the counter. Depending on the establishment, servings may be ordered from attendants, selected as ready-made portions already on plates, or self-serve their own portions. Cafeterias are common in hospitals, corporations and educational institutions. In Italy it's very common and known as \"mensa aziendale\". In the UK, a cafeteria may also offer a large selection of hot food and the use of the term cafeteria is deprecated in favour of self-service restaurant. Cafeterias have a wider variety of prepared foods. For example, it may have a variety of roasts (e.g. beef, ham, turkey) ready for carving by a server, as well as other main courses, rather than simple offerings of hamburgers or fried chicken."),
                ("Pub",
                    pub,
                    "Traditionally, pubs were primarily drinking establishments with food in a secondary position, whereas many modern pubs rely on food as well, to the point where gastropubs are often known for their high-quality fine-dining style pub food and concomitantly high prices. A typical pub has a large selection of beers and ales on tap."),
            };

            foreach (var category in categories)
            {
                await dbContext.Categories.AddAsync(new Category
                {
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    Name = category.Name,
                    Description = category.Description,
                    Title = category.Name,
                });
            }
        }
    }
}
