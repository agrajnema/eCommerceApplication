using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductManagementApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using ProductManagementApi.Filters;
using ProductManagementApi.Repository;
using ProductManagementApi.Models;
using InfrastructureLibrary;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProductManagementApi
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            AddRabbitMQ(services);

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add<JsonExceptionFilter>();
            //    options.Filters.Add<RequireHttpsOrCloseAttribute>();
            //});
            services.AddDbContext<ProductDBContext>(options => options.UseSqlServer(ReturnConnectionString()),ServiceLifetime.Transient);
            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Product Management" }));
            services.AddCors(options => options.AddPolicy("AllowDomain", policy => policy.AllowAnyOrigin()));
            services.AddScoped<IProductRepository, ProductRepository>();
            services
                .AddMvc(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson();

            var keySection = _configuration.GetSection("Settings");
            var key = keySection["SecretKey"];
            var signingKey = Encoding.ASCII.GetBytes(key);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = "AuthenticationKey";
            })
            .AddJwtBearer("AuthenticationKey", auth =>
            {
                auth.RequireHttpsMetadata = false;
                auth.TokenValidationParameters = tokenValidationParameters;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product Management API v1"));
            app.UseAuthentication();
            app.UseMvc();
            DBSeeder.PopulateDB(app);
        }

        private string ReturnConnectionString()
        {
            var server = _configuration["DBServer"] ?? "localhost";
            var port = _configuration["DBPort"] ?? "1433";
            var user = _configuration["DBUser"] ?? "SA";
            var password = _configuration["DBPassword"] ?? "Password@123";
            var database = _configuration["Database"] ?? "ProductDB";
            var connectionString = $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}";
            return connectionString;
        }

        private void AddRabbitMQ(IServiceCollection services)
        {
            var rabbitMQConfigSection = _configuration.GetSection("RabbitMQ");
            var userName = rabbitMQConfigSection["UserName"];
            var password = rabbitMQConfigSection["Password"];
            var host = rabbitMQConfigSection["Host"];
            var exchangeName = rabbitMQConfigSection["ExchangeName"];
            var exchangeType = rabbitMQConfigSection["ExchangeType"];
            services.AddTransient<IMessagePublisher>((mp) => new RabbitMQMessagePublisher(host, userName, password, exchangeName, exchangeType));
        }
    }
}
