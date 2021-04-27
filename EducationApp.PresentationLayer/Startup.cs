using EducationApp.BusinessLogicLayer;
using EducationApp.DataAccessLayer;
using EducationApp.DataAccessLayer.Initialization;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;
using System.Net;
using System.Threading.Tasks;

namespace EducationApp.PresentationLayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices();
            services.AddProviders();
            services.AddMapper();
            services.AddIdentity();

            services.AddDBContext(Configuration.GetConnectionString("DefaultConnection"));

            services.AddRepositories();

            var jwtConfig = Configuration.GetSection("jwt").Get<JwtConfig>();
            services.AddJwt(jwtConfig);


            services.Configure<SmtpConfig>(options => Configuration.GetSection("smtp").Bind(options));
            services.Configure<UrlConfig>(options => Configuration.GetSection("url").Bind(options));
            services.Configure<JwtConfig>(options => Configuration.GetSection("jwt").Bind(options));
            services.Configure<CurrencyConvertConfig>(options => Configuration.GetSection("currencyconvert").Bind(options));
            services.Configure<GoogleStorageConfig>(options => Configuration.GetSection("googlestorage").Bind(options));

            StripeConfiguration.ApiKey = Configuration["stripe:secretkey"];

            services.AddCors(options =>
            {
                options.AddPolicy(Constants.ALLOWSPECIFICORIGINS,
                builder =>
                {
                    builder.WithOrigins(Constants.DEFAULTFRONTENDORIGIN)
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToAccessDenied =
                    options.Events.OnRedirectToLogin = context =>
                    {
                        if (context.Request.Method != "GET")
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return Task.FromResult<object>(null);
                        }
                        context.Response.Redirect(context.RedirectUri);
                        return Task.FromResult<object>(null);
                    };
            });
            services.AddControllersWithViews();

            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.Seed();
            }

            app.UseMiddleware<Middlewares.LogMiddleware>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(Configuration.GetSection("swagger")["path"], Configuration.GetSection("swagger")["apiname"]);
            });

            app.UseRouting();
            app.UseCors(Constants.ALLOWSPECIFICORIGINS);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "AdminArea",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Admin}/{action=Login}");
                endpoints.MapAreaControllerRoute(
                   name: "default",
                   areaName: "Admin",
                   pattern: "{controller=Admin}/{action=Login}");
                endpoints.MapControllers();

            });
        }
    }
}
