using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.AppContext
{
    public class ApplicationContext : IdentityDbContext<UserEntity>
    {
        private readonly StreamWriter _logStream;
        public virtual DbSet<AuthorEntity> Authors { get; set; }
        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<PaymentEntity> Payments { get; set; }
        public virtual DbSet<PrintingEditionEntity> PrintingEditions { get; set; }
        public virtual DbSet<OrderItemEntity> OrderItems { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AuthorEntity>()
                    .HasMany(edition => edition.PrintingEditions)
                    .WithMany(author => author.Authors)
                    .UsingEntity(join => join.ToTable(Constants.DEFAULTJOINTTABLENAME));
            modelBuilder.Entity<PrintingEditionEntity>()
                .Property(edition => edition.SubTitle)
                .HasDefaultValue("empty");
            base.OnModelCreating(modelBuilder);
        }
    }
}
