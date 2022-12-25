using CMSApplication.Data.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CMSApplication.Data.Configs
{
    public class UserRoleConfig : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {

            //builder.Ignore(x => x.RoleId);
            //builder.Ignore(x => x.UserId);

            builder
                .HasOne(x => x.AppRole)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.RoleId);

            builder.HasOne(x => x.userRole)
                .WithMany(x => x.UserRoles)
                .HasForeignKey(x => x.UserId);

            builder
                .Ignore(x => x.CreatedDate)
                .Ignore(x => x.ModifiedDate)
                ;
        }
    }
}
