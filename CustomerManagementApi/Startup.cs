using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagementApi.Services;
using CustomerManagementApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CustomerManagementApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CustomerManagementApi.Model;
using InfrastructureLibrary;

namespace CustomerManagementApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration _configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddDbContext<CustomerDBContext>(options => options.UseSqlServer(ReturnConnectionString()),ServiceLifetime.Transient);
            //services.AddCors(options => options.AddPolicy("AllowDomain", policy => policy.AllowAnyOrigin()));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson();
            AddRabbitMQ(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseHttpsRedirection();

            //app.UseRouting();


            //app.UseAuthorization();
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseMvc();
            DBSeeder.PopulateDB(app);
        }

        private string ReturnConnectionString()
        {
            var server = _configuration["DBServer"] ?? "localhost";
            var port = _configuration["DBPort"] ?? "1433";
            var user = _configuration["DBUser"] ?? "SA";
            var password = _configuration["DBPassword"] ?? "Password@123";
            var database = _configuration["Database"] ?? "CustomerDB";
            var connectionString = $"Server={server},{port};Initial Catalog={database}; User ID={user}; Password={password}";
            return connectionString;
        }

        private void AddRabbitMQ(IServiceCollection services)
        {
            var rabbitMQConfigSection = _configuration.GetSection("RabbitMQ");
            var userName = rabbitMQConfigSection["UserName"];
            var password = rabbitMQConfigSection["Password"];
            var host = rabbitMQConfigSection["Host"] ?? "localhost";
            var hostList = new List<string>();
            hostList.Add(host);
            hostList.Add("localhost");
            var port = Convert.ToInt32(rabbitMQConfigSection["Port"]);
            var exchangeName = rabbitMQConfigSection["ExchangeName"];
            var exchangeType = rabbitMQConfigSection["ExchangeType"];
            services.AddTransient<IMessagePublisher>((mp) => new RabbitMQMessagePublisher(hostList, userName, password, exchangeName, exchangeType));
        }
    }
}
