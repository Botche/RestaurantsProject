namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAdministrationService
    {
        Task PromoteAsync(string id, string roleName);

        Task DemoteAsync(string id, string roleName);

        Task BanAsync(string id);
    }
}
