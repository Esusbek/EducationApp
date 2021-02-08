using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EducationApp.DataAccessLayer.Entities;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EducationApp.DataAccessLayer.AppContext
{
    class ApplicationContext: IdentityDbContext<Entities.User>
    {
        RoleManager<IdentityRole> roleManager;
        public virtual DbSet<AuthorEntity> Authors { get; set; }
        public virtual DbSet<OrderEntity> Orders { get; set; }
        public virtual DbSet<PaymentEntity> Payments { get; set; }
        public virtual DbSet<PrintingEditionEntity> PrintingEditions { get; set; }
        public virtual DbSet<OrderItemEntity> OrderItems { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            :base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("../appsettings.json");
                var config = builder.Build();
                string connectionString = config.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
        
    }
}
