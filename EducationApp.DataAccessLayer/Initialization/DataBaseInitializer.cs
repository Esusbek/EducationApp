using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer.Initialization
{
    public class DataBaseInitializer
    {
        private AppContext.ApplicationContext _appContext;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DataBaseInitializer(UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager,
            AppContext.ApplicationContext appContext)
        {
            _appContext = appContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            var existingPE = _appContext.PrintingEditions.FirstOrDefault(pe => pe.Title == "Test PE");
            if(_appContext.Authors.Any())
            {
                return;
            }
            _appContext.Set<PrintingEditionEntity>().Add(new PrintingEditionEntity
                {
                    Id = 1,
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
                Id = 1,
                Name = "Vasily",
                IsRemoved = false,
                PrintingEditions = new List<PrintingEditionEntity>()
            });
            _appContext.SaveChanges();
            await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = "admin"
                });
            await _roleManager.CreateAsync(new IdentityRole
            {
                Name = "client"
            });

            await _userManager.CreateAsync(new UserEntity
            {
                FirstName = "Vitaly",
                LastName = "Pupkin",
                Email = "admin@test.com",
                UserName = "Admin",
                EmailConfirmed = true,
                IsRemoved = false
            }, "securePassword");
            var book = _appContext.PrintingEditions.SingleOrDefault(pe => pe.Title == "Test PE");
            var author = book.Authors.SingleOrDefault(a => a.Name == "Vasily");
            if(author==null)
            {
                book.Authors.Add(_appContext.Authors.Single(a => a.Name == "Vasily"));
            }
            _appContext.SaveChanges();
        }
    }
}
