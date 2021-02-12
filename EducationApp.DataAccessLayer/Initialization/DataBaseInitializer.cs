using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Initialization
{
    public static class DataBaseInitializer
    {

        public static void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _appContext = serviceScope.ServiceProvider.GetService<AppContext.ApplicationContext>();
                _appContext.Database.EnsureCreated();

                var _userManager =
                         serviceScope.ServiceProvider.GetService<UserManager<UserEntity>>();
                var _roleManager =
                         serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                if (_appContext.Authors.Any())
                {
                    return;
                }
                _appContext.Set<PrintingEditionEntity>().Add(new PrintingEditionEntity
                {
                    IsRemoved = false,
                    Currency = Enums.Currency.UAH,
                    Price = 200.05M,
                    Description = "Initial test PE",
                    Title = "Test PE",
                    Type = Enums.PrintingEdition.Type.Book,
                    Status = Enums.PrintingEdition.Status.InStock
                });
                _appContext.Set<AuthorEntity>().Add(new AuthorEntity
                {
                    Name = "Vasily",
                    IsRemoved = false,
                    PrintingEditions = new List<PrintingEditionEntity>()
                });
                _appContext.SaveChanges();
                var adminRole = _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "admin"
                }).Result;
                var clientRole = _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "client"
                }).Result;

                

                var userResult = _userManager.CreateAsync(new UserEntity
                {
                    FirstName = "Vitaly",
                    LastName = "Pupkin",
                    Email = "admin@test.com",
                    UserName = "Admin",
                    EmailConfirmed = true,
                    IsRemoved = false
                }, "secureP@ssword123").Result;

                var adminUser = _userManager.FindByNameAsync("Admin").Result;

                var userRole = _userManager.AddToRoleAsync(adminUser, "admin").Result;
                var author = _appContext.Authors.SingleOrDefault(a => a.Name == "Vasily");
                var book = author.PrintingEditions.SingleOrDefault(pe => pe.Title == "Test PE");
                if (book == null)
                {
                    author.PrintingEditions.Add(_appContext.PrintingEditions.Single(pe => pe.Title == "Test PE"));
                }
                _appContext.SaveChanges();
            }
        }
    }
}
