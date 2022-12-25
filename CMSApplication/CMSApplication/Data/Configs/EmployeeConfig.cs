using CMSApplication.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMSApplication.Data.Configs
{
    public class EmployeeConfig :IEntityTypeConfiguration<Employee>
    {
       public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(255);
            builder.Property(x => x.ContactNo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.BaseLocation).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ContactNo).IsRequired().HasMaxLength(20);
            builder.Property(x => x.PrimarySkill).IsRequired();
            builder.Property(x => x.Feedback).HasMaxLength(100);
        }
    }
}
