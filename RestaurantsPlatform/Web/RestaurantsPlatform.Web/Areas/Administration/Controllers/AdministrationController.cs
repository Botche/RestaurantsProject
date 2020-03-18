namespace RestaurantsPlatform.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using RestaurantsPlatform.Common;
    using RestaurantsPlatform.Web.Controllers;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}
