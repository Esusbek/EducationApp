using AutoMapper;
using EducationApp.BusinessLogicLayer.Common.MappingProfiles;
using EducationApp.BusinessLogicLayer.Providers;
using EducationApp.BusinessLogicLayer.Providers.Interfaces;
using EducationApp.BusinessLogicLayer.Services;
using EducationApp.BusinessLogicLayer.Services.Interfaces;
using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Entities;
using EducationApp.Shared.Configs;
using EducationApp.Shared.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace EducationApp.BusinessLogicLayer
{
    public static class StartupExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IPrintingEditionService, PrintingEditionService>();
            services.AddTransient<IOrderService, OrderService>();
        }

        public static void AddProviders(this IServiceCollection services)
        {
            services.AddTransient<IJwtProvider, JwtProvider>();
            services.AddTransient<ICurrencyConvertionProvider, CurrencyConvertionProvider>();
            services.AddTransient<IUserValidationProvider, UserValidationProvider>();
            services.AddTransient<IEmailProvider, EmailProvider>();
        }

        public static void AddMapper(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles.UserMapProfile());
                cfg.AddProfile(new MappingProfiles.AuthorMapProfile());
                cfg.AddProfile(new MappingProfiles.PrintingEditionMapProfile());
                cfg.AddProfile(new MappingProfiles.OrderMapProfile());
            });
            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void AddIdentity(this IServiceCollection services)
        {
            services.AddIdentity<UserEntity, IdentityRole>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
                config.User.RequireUniqueEmail = true;
                config.Password.RequireDigit = true;
                config.Password.RequiredLength = Constants.DEFAULTMINPASSWORDLENGTH;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = true;
                config.Password.RequireLowercase = true;
            })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();
        }

        public static void AddJwt(this IServiceCollection services, JwtConfig jwtConfig)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = true;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Key)),
                    ValidAudience = jwtConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(jwtConfig.ClockSkew)
                };
            });
        }
    }
}
