using EducationApp.DataAccessLayer.AppContext;
using EducationApp.DataAccessLayer.Repositories;
using EducationApp.DataAccessLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationApp.DataAccessLayer
{
    public static class StartupExtension
    {
        public static void AddDBContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString));
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IOrderItemRepository, OrderItemRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<IPrintingEditionRepository, PrintingEditionRepository>();
        }
    }
}
