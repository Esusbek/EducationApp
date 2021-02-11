using EducationApp.BusinessLogicLayer.Services;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EducationApp.PresentationLayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<UserEntity, IdentityRole>(config =>
                {
                    config.SignIn.RequireConfirmedEmail = true;
                    config.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
            services.Configure<SmtpConfig>(options => Configuration.GetSection("smtp").Bind(options));
            services.Configure<UrlConfig>(options => Configuration.GetSection("url").Bind(options));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<Middlewares.LogMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Action}/{action=Register}");
                endpoints.MapControllerRoute(
                    name: "confirm",
                    pattern: "{controller=Action}/{action=ConfirmEmail}");
                endpoints.MapControllerRoute(
                   name: "confirm",
                   pattern: "{controller=Action}/{action=Login}");
            });
        }
    }
}
