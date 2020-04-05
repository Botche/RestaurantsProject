using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(RestaurantsPlatform.Web.Areas.Identity.IdentityHostingStartup))]

namespace RestaurantsPlatform.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
