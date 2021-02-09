using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.AppContext
{
    class ApplicationContext: IdentityDbContext<UserEntity>
    {
        private readonly StreamWriter _logStream = new StreamWriter("log.txt", true);
        public virtual DbSet<AuthorEntity> Authors { get; set; }
        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<PaymentEntity> Payments { get; set; }
        public virtual DbSet<PrintingEditionEntity> PrintingEditions { get; set; }
        public virtual DbSet<OrderItemEntity> OrderItems { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json");
                var config = builder.Build();
                string connectionString = config.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
            optionsBuilder.LogTo(_logStream.WriteLine, Microsoft.Extensions.Logging.LogLevel.Debug);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var hasher = new PasswordHasher<UserEntity>();

            modelBuilder.Entity<AuthorEntity>()
                    .HasMany(c => c.PrintingEditions)
                    .WithMany(s => s.Authors)
                    .UsingEntity(j => j.ToTable("AuthorInPrintingEditions"));
            //modelBuilder.Entity<PrintingEditionEntity>().HasData(new PrintingEditionEntity 
            //{ 
            //    Id = 1,
            //    IsRemoved = false,
            //    Currency = Entities.Enums.Enums.Currency.UAH, 
            //    Price = 200.05M, 
            //    Description = "Initial test PE", 
            //    Title = "Test PE", 
            //    Type = Entities.Enums.Enums.PrintingEdition.Type.Book, 
            //    Status = Entities.Enums.Enums.PrintingEdition.Status.InStock
            //});
            //modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
            //{
            //    Id = "1",
            //    Name = "Admin",
            //    NormalizedName = "Admin".ToLower()
            //});
            //modelBuilder.Entity<AuthorEntity>().HasData(new AuthorEntity
            //{
            //    Id = 1,
            //    Name = "Vasily",
            //    IsRemoved = false
            //});
            //modelBuilder.Entity<UserEntity>().HasData(new UserEntity
            //{
            //    Id = "1",
            //    FirstName = "Vitaly",
            //    LastName = "Pupkin",
            //    Email = "test@test.com",
            //    UserName = "Admin",
            //    NormalizedUserName = "Admin".ToLower(),
            //    PasswordHash = hasher.HashPassword(null, "password"),
            //    NormalizedEmail = "test@test.com".ToLower(),
            //    EmailConfirmed = true,
            //    SecurityStamp = string.Empty
            //});
            //modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            //{
            //    RoleId = "1",
            //    UserId = "1"
            //});

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
