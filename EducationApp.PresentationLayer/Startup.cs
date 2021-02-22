using EducationApp.BusinessLogicLayer;
using EducationApp.DataAccessLayer;
using EducationApp.DataAccessLayer.Initialization;
using EducationApp.Shared.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

namespace EducationApp.PresentationLayer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "AllowSpecificOrigins";

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

            StripeConfiguration.ApiKey = Configuration["stripe:secretkey"];

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("http://localhost:53135",
                                        "http://localhost:4200")
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                });
            });

            services.AddControllers();

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
                DataBaseInitializer.Seed(app);
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
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Account}/{action=Login}");
            });
        }
    }
}
