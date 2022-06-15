using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementApi.Clean.Application.Contracts.Infrastucture;
using OrderManagementApi.Clean.Application.Contracts.Persistence;
using OrderManagementApi.Clean.Application.Models;
using OrderManagementApi.Clean.Infrastructure.Mail;
using OrderManagementApi.Clean.Infrastructure.Persistence;
using OrderManagementApi.Clean.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementApi.Clean.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options => options.UseSqlServer(configuration.GetConnectionString("OrderManagementApiConnectionString")));
            services.AddScoped(typeof(IGenericAsyncRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.Configure<EmailSettings>(e => configuration.GetSection("EmailSettings"));
            services.AddTransient<IEmailService, EmailService>();
            return services;
        }
    }
}
