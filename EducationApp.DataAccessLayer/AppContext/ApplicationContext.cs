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
            _logStream = new StreamWriter(Constants.LogDestinations.DBLogs, true);
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            _logStream = new StreamWriter(Constants.LogDestinations.DBLogs, true);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(_logStream.WriteLine, Microsoft.Extensions.Logging.LogLevel.Warning);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorEntity>()
                    .HasMany(c => c.PrintingEditions)
                    .WithMany(s => s.Authors)
                    .UsingEntity(j => j.ToTable("AuthorInPrintingEditions"));

            base.OnModelCreating(modelBuilder);
        }
        public override void Dispose()
        {
            base.Dispose();
            _logStream.Dispose();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _logStream.DisposeAsync();
        }
    }
}
