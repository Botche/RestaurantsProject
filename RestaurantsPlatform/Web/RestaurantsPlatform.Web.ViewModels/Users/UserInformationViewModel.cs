namespace RestaurantsPlatform.Web.ViewModels.Users
{
    using System;
    using System.Linq;

    using AutoMapper;

    using RestaurantsPlatform.Data.Models;
    using RestaurantsPlatform.Services.Mapping;

    public class UserInformationViewModel : IHaveCustomMappings
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedOn { get; set; }

        public string RoleName { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UserInformationViewModel>()
                .ForMember(dest => dest.RoleName, ori => ori.MapFrom(dest => dest.UserRoles.FirstOrDefault().Role.Name));
        }
    }
}
