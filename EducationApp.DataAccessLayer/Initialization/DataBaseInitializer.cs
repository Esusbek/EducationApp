using EducationApp.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;


namespace EducationApp.DataAccessLayer.Initialization
{
    public class DataBaseInitializer
    {
        public static void DatabaseInitialization()
        {
            var hasher = new PasswordHasher<UserEntity>();
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
            //    SecurityStamp = string.Empty,
            //    IsRemoved = false
            //});
            //modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            //{
            //    RoleId = "1",
            //    UserId = "1"
            //});

        }
    }
}
