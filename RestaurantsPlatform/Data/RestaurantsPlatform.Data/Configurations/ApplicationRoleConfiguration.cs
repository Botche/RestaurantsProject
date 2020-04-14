namespace RestaurantsPlatform.Data.Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    using RestaurantsPlatform.Data.Models;

    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> appRole)
        {
            appRole
                .HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(e => e.RoleId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
