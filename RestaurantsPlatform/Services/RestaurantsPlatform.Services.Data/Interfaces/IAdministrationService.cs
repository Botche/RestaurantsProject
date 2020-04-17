namespace RestaurantsPlatform.Services.Data.Interfaces
{
    using System.Threading.Tasks;

    public interface IAdministrationService
    {
        Task PromoteAsync(string id, string roleName);

        Task DemoteAsync(string id, string roleName);

        Task BanAsync(string id);

        Task UnBanAsync(string id);
    }
}
