using CMSApplication.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMSApplication.Data.Configs
{
    public class QuizConfig : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder
                .HasKey(x => x.Id)
                ;


            builder
                .Property(x => x.Id).ValueGeneratedOnAdd();

            builder
                .Property(x => x.description)
                .IsRequired(false)
                .HasMaxLength(5000);


        }
    }
}
