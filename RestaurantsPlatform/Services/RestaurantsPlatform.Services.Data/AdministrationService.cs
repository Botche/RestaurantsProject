namespace RestaurantsPlatform.Services.Data
{
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using RestaurantsPlatform.Data.Common.Repositories;
    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Data.Interfaces;

    using static RestaurantsPlatform.Common.GlobalConstants;

    public class AdministrationService : IAdministrationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public AdministrationService(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.userManager = userManager;
            this.userRepository = userRepository;
        }

        public async Task BanAsync(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            await this.userManager.SetLockoutEnabledAsync(user, true);

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
        }

        public async Task DemoteAsync(string id, string roleName)
        {
            string newRole = this.DeterminateLowerRole(roleName);

            await this.PromoteDemoteAsync(id, roleName, newRole);
        }

        public async Task PromoteAsync(string id, string roleName)
        {
            string newRole = this.DeterminateNextRole(roleName);

            await this.PromoteDemoteAsync(id, roleName, newRole);
        }

        private async Task PromoteDemoteAsync(string id, string oldRole, string newRole)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (!string.IsNullOrEmpty(oldRole))
            {
                await this.userManager.RemoveFromRoleAsync(user, oldRole);
            }

            await this.userManager.AddToRoleAsync(user, newRole);

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();
        }

        private string DeterminateNextRole(string roleName)
        {
            string newRole = string.Empty;

            if (roleName == UserRoleName)
            {
                newRole = RestaurantRoleName;
            }
            else if (roleName == RestaurantRoleName)
            {
                newRole = AdministratorRoleName;
            }
            else if (string.IsNullOrEmpty(roleName))
            {
                newRole = UserRoleName;
            }

            return newRole;
        }

        private string DeterminateLowerRole(string roleName)
        {
            string newRole = string.Empty;

            if (roleName == AdministratorRoleName)
            {
                newRole = RestaurantRoleName;
            }
            else if (roleName == RestaurantRoleName)
            {
                newRole = UserRoleName;
            }

            return newRole;
        }
    }
}
