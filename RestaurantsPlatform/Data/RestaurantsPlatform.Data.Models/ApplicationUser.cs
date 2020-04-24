// ReSharper disable VirtualMemberCallInConstructor
namespace RestaurantsPlatform.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    using RestaurantsPlatform.Data.Common.Models;
    using RestaurantsPlatform.Data.Models.Restaurants;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserRoles = new HashSet<ApplicationUserRole>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Restaurants = new HashSet<Restaurant>();
            this.Comments = new HashSet<Comment>();
            this.Votes = new HashSet<Vote>();
            this.FavouriteRestaurants = new HashSet<FavouriteRestaurant>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string ImageUrl { get; set; }

        public string PublicId { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<FavouriteRestaurant> FavouriteRestaurants { get; set; }
    }
}
