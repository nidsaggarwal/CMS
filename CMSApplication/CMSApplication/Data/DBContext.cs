using CMSApplication.Data.Configs;
using CMSApplication.Data.Entity;
using CMSApplication.Services.Abstraction;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CMSApplication.Data
{
    public class DBContext : IdentityDbContext<User,IdentityRole,string>
    {
        public DBContext(DbContextOptions options): base(options)
        {

        }

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Working> Workings { get; set; }
        public virtual DbSet<InternalEvaluation> InternalEvaluations { get; set; }
        public virtual DbSet<ClientEvaluation> ClientEvaluations { get; set; }

     

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new EmployeeConfig());
            builder.ApplyConfiguration(new WorkingConfig());
            builder.ApplyConfiguration(new InternalEvaluationConfig());
            builder.ApplyConfiguration(new ClientEvaluationConfig());
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker.Entries();

            var now = DateTime.UtcNow;
            foreach (var item in entries)
            {
                if (item.Entity is IBaseEntity)
                {
                    switch (item.State)
                    {
                        case EntityState.Modified:
                            {
                                item.Property(nameof(IBaseEntity.ModifiedAt)).CurrentValue = now;
                            }
                            break;
                        case EntityState.Added:
                            {
                                item.Property(nameof(IBaseEntity.CreatedAt)).CurrentValue = now;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
